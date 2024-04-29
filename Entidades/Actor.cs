namespace MinimalAPIPeliculas.Entidades
{
    public class Actor
    {            
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string? Foto { get; set; }
        //Propiedad de navegación de Actor
        public List<ActorPelicula> ActoresPeliculas { get; set; } = new List<ActorPelicula>();
    }
}

