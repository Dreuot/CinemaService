using BackService.AsyncConsumers;

namespace BackService.EventProcessings
{
    public abstract class AsyncEventProcessor : IAsyncEventProcessor
    {
        public abstract string ProcessEvent(AsyncMessage message);

        public static IAsyncEventProcessor GetProcessor(AsyncMessage message, IServiceScopeFactory scopeFactory)
        {
            switch (message.Type)
            {
                case "GetAllMovies":
                    return new GetAllMoviesProcessor(scopeFactory);
                case "GetCinemaMovieSeances":
                    return new GetCinemaMovieSeances(scopeFactory);
                default:
                    throw new ArgumentOutOfRangeException($"Unknown message type '{message.Type}'");
            }
        }
    }
}
