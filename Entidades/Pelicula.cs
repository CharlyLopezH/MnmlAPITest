namespace MinimalAPIPeliculas.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public bool EnCines { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string? Poster { get; set; }
        //Definición de la relacion 1 a muchos con la entidad Películas
        //Una Película muchos comentarios.
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();

        //Para la relacion m-m con Generos
        public List<GeneroPelicula> GenerosPeliculas { get; set; } = new List<GeneroPelicula>();
        //Para la relación m-m con Actores 
        public List<ActorPelicula> ActoresPeliculas { get; set; } = new List<ActorPelicula>();
    }
}
