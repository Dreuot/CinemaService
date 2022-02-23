namespace BackService.Data.Models
{
    public class CinemaMovie
    {
        public int CinemaId { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public Cinema Cinema { get; set; }
    }
}
