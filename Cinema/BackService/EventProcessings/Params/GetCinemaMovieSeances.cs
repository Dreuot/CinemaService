namespace BackService.EventProcessings.Params
{
    public record GetCinemaMovieSeances(
        int CinemaId,
        int MovieId
    );
}
