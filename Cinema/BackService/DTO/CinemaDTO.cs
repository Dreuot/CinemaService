namespace BackService.DTO
{
    public class CinemaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public IEnumerable<SeanceDTO> Seances { get; set; }
    }
}
