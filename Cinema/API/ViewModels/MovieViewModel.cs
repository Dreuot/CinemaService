namespace API.ViewModels
{
    //public class MovieViewModel
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //}
    public record MovieViewModel(
        int Id,
        string Name,
        string Description,
        IEnumerable<MovieCinemaViewModel> Cinemas
    );
}
