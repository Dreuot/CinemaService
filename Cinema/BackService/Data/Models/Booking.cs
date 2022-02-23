namespace BackService.Data.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SeanceId { get; set; }
        public Seance Seance { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; }
    }
}
