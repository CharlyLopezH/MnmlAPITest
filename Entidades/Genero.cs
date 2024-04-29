namespace MinimalAPIPeliculas.Entidades
{
    public class Genero
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null! ;

        //Para la relacion m-m con Peliculas
        public List<GeneroPelicula> GenerosPeliculas { get; set; } = new List<GeneroPelicula>();
    }
}
