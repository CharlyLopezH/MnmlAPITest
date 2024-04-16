using Microsoft.EntityFrameworkCore;
using MinimalAPIPeliculas.Entidades;

namespace MinimalAPIPeliculas
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        //Polimorfisto para la configuración de propiedades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Configuración de propiedades
            modelBuilder.Entity<Actor>().Property(p=>p.Nombre).HasMaxLength(150);
            modelBuilder.Entity<Actor>().Property(p => p.Foto).IsUnicode();
            modelBuilder.Entity<Pelicula>().Property(p => p.Titulo).HasMaxLength(160);
            modelBuilder.Entity<Pelicula>().Property(p=>p.Poster).IsUnicode();
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set;}
    }
}
