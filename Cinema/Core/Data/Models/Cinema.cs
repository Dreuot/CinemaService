namespace Core.Data.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public IQueryable<Film> Films { get; set; }
    }
}
