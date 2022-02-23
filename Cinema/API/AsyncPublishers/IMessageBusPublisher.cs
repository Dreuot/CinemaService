namespace API.AsyncPublishers
{
    public interface IMessageBusPublisher : IDisposable
    {
        public string GetAllMovies();
        public Task<string> GetAllMoviesAsync();
        public Task<string> GetCinemaMovieSeances(int cinemaId, int movieId);
    }
}
