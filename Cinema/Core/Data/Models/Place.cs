namespace Core.Data.Models
{
    public class Place
    {
        public int Id { get; set; }
        public int HallId { get; set; }
        public Hall Hall { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
    }
}
