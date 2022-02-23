namespace BackService.Data.Models
{
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public ICollection<Seance> Seances { get; set; }
        public ICollection<Place> Places { get; set; }
    }
}
