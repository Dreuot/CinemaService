namespace Core.Data.Models
{
    public class Seance
    {
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
        public DateTime BeginTime { get; set; }
        public int Duration { get; set; }
    }
}
