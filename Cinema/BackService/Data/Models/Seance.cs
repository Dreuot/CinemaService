namespace BackService.Data.Models
{
    public class Seance
    {
        public int Id { get; set; }
        public int HallId { get; set; }
        public Hall Hall { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public DateTime BeginTime { get; set; }
        public int Duration { get; set; }
    }
}