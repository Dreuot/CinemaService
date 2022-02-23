namespace BackService.Data.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Movie> Movies { get; set; }
        public ICollection<CinemaMovie> CinemaMovies { get; set; }
        public ICollection<Hall> Halls { get; set; }
    }
}
