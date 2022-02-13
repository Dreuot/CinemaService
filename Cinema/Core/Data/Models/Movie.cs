namespace Core.Data.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Cinema> Cinemas { get; set; }
        public ICollection<CinemaMovie> CinemaMovies { get; set; }
    }
}
