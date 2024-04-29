using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinimalAPIPeliculas.Entidades;

namespace MinimalAPIPeliculas
{
    //Heredar de identity para agregar automáticamente funciones de Identity roles, etc... (:Dbcontext ya no)
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        //Polimorfisto para la configuración de propiedades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Necesariio para las migraciones
            base.OnModelCreating(modelBuilder);
            //Configuración de propiedades
            modelBuilder.Entity<Actor>().Property(p=>p.Nombre).HasMaxLength(150);
            modelBuilder.Entity<Actor>().Property(p => p.Foto).IsUnicode();
            modelBuilder.Entity<Pelicula>().Property(p => p.Titulo).HasMaxLength(160);
            modelBuilder.Entity<Pelicula>().Property(p=>p.Poster).IsUnicode();
            //Para la relacion m - m
            modelBuilder.Entity<GeneroPelicula>().HasKey(g => new { g.GeneroId, g.PeliculaId });
            modelBuilder.Entity<ActorPelicula>().HasKey(a => new {a.PeliculaId, a.ActorId});

            //Agregar para personalizar tablas
            modelBuilder.Entity<IdentityUser>().ToTable("Usuarios");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UsuariosClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UsuariosLogins");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UsuariosRoles");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsuariosTokens");

        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set;}
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<GeneroPelicula> GenerosPeliculas { get; set; }
        public DbSet<ActorPelicula> ActoresPeliculas { get;set; }
        public DbSet<Error> Errores { get; set; }

    }
}
