using Microsoft.EntityFrameworkCore;
using MinimalAPIPeliculas.DTOs;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Utilidades;
using System.Runtime.CompilerServices;

namespace MinimalAPIPeliculas.Repositorios
{
    public class RepositorioActores : IRepositorioActores
    {
        private readonly ApplicationDbContext context;        
        private readonly HttpContext httpContext;

        public RepositorioActores(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor )
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }
        //Implementación de las operaciones REST pare CRUD de la Tabla.
        //Los Repositorios se entienden con las clases directamente

        public async Task<List<Actor>> ObtenerTodos(PaginacionDTO paginacionDTO)
        {
            var queryable = context.Actores.AsQueryable();
            await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
        }

        public async Task<Actor?> ObtenerPorId(int id)
        {
            return await context.Actores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Crear(Actor actor)
        {
            context.Add(actor);
            await context.SaveChangesAsync();
            return actor.Id;

        }

        public async Task<bool> Existe(int id)
        {
            return await context.Actores.AnyAsync(x => x.Id == id);
        }

        public async Task Actualizar(Actor actor)
        {
            context.Update(actor);
            await context.SaveChangesAsync();
        }

        public async Task Borrar(int id)
        {
            await context.Actores.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Actor>> ObtenerPorNombre(string nombre)
        {
            return await context.Actores
                .Where(a=>a.Nombre.Contains(nombre))
                .OrderBy(a=>a.Nombre).ToListAsync();
        }

    }
}
