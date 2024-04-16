using MinimalAPIPeliculas.Entidades;

namespace MinimalAPIPeliculas.Repositorios
{
    public class RepositorioPeliculas
    {
        private readonly ApplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositorioPeliculas(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;
        }

        //public async Task<List<Pelicula>> ObtenerTodos(PaginacionDTO paginacionDTO)
        //{
        //    var queryable = context.Peliculas.AsQueryable();
        //    await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
        //    return await queryable.OrderBy(p => p.Titulo).Paginar(paginacionDTO).ToListAsync();
        //}

    }
}
