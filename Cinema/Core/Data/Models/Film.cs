namespace Core.Data.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IQueryable<Cinema> Cinemas { get; set; }
    }
}
