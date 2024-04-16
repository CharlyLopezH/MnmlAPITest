﻿using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entidades;
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
            group.MapPost("/", Crear).DisableAntiforgery();
            group.MapGet("/", ObtenerActores).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(50)).Tag("actores-tag-cache"));
            return group;
        }
        static async Task<Created<ActorDTO>> Crear([FromForm] CrearActorDTO crearActorDTO, 
            IRepositorioActores repositorio, IOutputCacheStore outputCacheStore, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
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

        
        static async Task<Ok<List<ActorDTO>>> ObtenerActores(IRepositorioActores repositorio, IMapper mapper)
        {
            var actores = await repositorio.ObtenerTodos();
            var actoresDTO = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresDTO);
        }
    }
}
