namespace MinimalAPIPeliculas.Entidades
{
    public class ActorPelicula
    {
        public int ActorId { get; set; }
        public int PeliculaId { get; set; }
        //Propiedades de Navegación
        public Actor Actor { get; set; } = null!;
        public Pelicula Pelicula { get; set; } = null!;  
        public int Orden { get; set; }
        public string Personaje { get; set; } = null!;
    }
}
