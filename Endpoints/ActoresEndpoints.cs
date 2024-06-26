﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Filtros;
using MinimalAPIPeliculas.Migrations;
using MinimalAPIPeliculas.Repositorios;
using MinimalAPIPeliculas.Servicios;

namespace MinimalAPIPeliculas.Endpoints
{
    public static class ActoresEndpoints
    {
        public static readonly string contenedor = "actores";
        public static RouteGroupBuilder MapActores(this RouteGroupBuilder group)
        {
            group.MapPost("/", Crear).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearActorDTO>>();
            group.MapGet("/", ObtenerTodos).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(50)).Tag("actores-tag-cache"));
            group.MapPut("/{id:int}", Actualizar).DisableAntiforgery().AddEndpointFilter<FiltroValidaciones<CrearActorDTO>>(); ;
            group.MapGet("/{id:int}", ObtenerPorId);
            group.MapDelete("/{id:int}", Borrar);
            group.MapGet("/obtenerPorNombre/{nombre}", ObtenerPorNombre);
            return group;
        }

    static async Task<Results<Created<ActorDTO>,ValidationProblem>> Crear([FromForm] CrearActorDTO crearActorDTO, IRepositorioActores repositorio, 
                                                IOutputCacheStore outputCacheStore, 
                                                IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
          
            var actor = mapper.Map<Actor>(crearActorDTO);
            if (crearActorDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearActorDTO.Foto);
                actor.Foto = url;
            }
            var id = await repositorio.Crear(actor);
            await outputCacheStore.EvictByTagAsync("actores-tag-cache", default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Created($"/actores/{id}", actorDTO);
        }

        static async Task<Ok<List<ActorDTO>>> ObtenerTodos(IRepositorioActores repositorio, IMapper mapper, int pagina=1, int recordsPorPagina=10)
        {
            var paginacion=new PaginacionDTO {Pagina= pagina, RecordsPorPagina=recordsPorPagina};
            var actores = await repositorio.ObtenerTodos(paginacion);
            var actoresDTO = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresDTO);
        }

        static async Task<Results<Ok<ActorDTO>, NotFound>> ObtenerPorId(int id, 
            IRepositorioActores repositorio, IMapper mapper)
        {
            var actor = await repositorio.ObtenerPorId(id);
            if (actor is null)
            {
                return TypedResults.NotFound();
            }
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Ok(actorDTO);
        }

        //En este caso no se usa un DTO para actualizar; sino la clase CrearActor
        static async Task<Results<NoContent, NotFound>> Actualizar(int id,
        [FromForm] CrearActorDTO crearActorDTO, IRepositorioActores repositorio,
        IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var actorDB = await repositorio.ObtenerPorId(id);

            //Atención debido a que es una actualización necesitamos verificar que exista el registro que vamos a modificar y recuperar sus datos
            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            var actorParaActualizar = mapper.Map<Actor>(crearActorDTO);
            actorParaActualizar.Id = id;
            actorParaActualizar.Foto = actorDB.Foto;

            if (crearActorDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Editar(actorParaActualizar.Foto,
                    contenedor, crearActorDTO.Foto);
                actorParaActualizar.Foto = url;
            }

            await repositorio.Actualizar(actorParaActualizar);
            await outputCacheStore.EvictByTagAsync("actores-tag-cache", default);
            return TypedResults.NoContent();
        }


        //Borrar
        static async Task<Results<NoContent, NotFound>> Borrar(int id, IRepositorioActores repositorio,
            IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
        {
            var actorDB = await repositorio.ObtenerPorId(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            await repositorio.Borrar(id);
            await almacenadorArchivos.Borrar(actorDB.Foto, contenedor);
            await outputCacheStore.EvictByTagAsync("actores-tag-cache", default);
            return TypedResults.NoContent();
        }
        //Buscar por nombre
        static async Task<Ok<List<ActorDTO>>> ObtenerPorNombre(string nombre, IRepositorioActores repositorio, IMapper mapper)
        {
            var actores = await repositorio.ObtenerPorNombre(nombre);
            var actoresDTO = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresDTO);
        }
    }
}
