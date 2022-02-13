namespace Core.Data.Models
{
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CimenaId { get; set; }
        public Cinema Cinema { get; set; }
    }
}
