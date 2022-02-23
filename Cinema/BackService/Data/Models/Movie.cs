namespace BackService.Data.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Cinema> Cinemas { get; set; }
        public ICollection<CinemaMovie> CinemaMovies { get; set; }
        public ICollection<Seance> Seances { get; set; }
    }
}
