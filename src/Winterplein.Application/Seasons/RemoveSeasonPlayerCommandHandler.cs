using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Seasons;

public static class RemoveSeasonPlayerCommandHandler
{
    public static void Handle(RemoveSeasonPlayerCommand request, ISeasonRepository seasonRepository)
    {
        var season = seasonRepository.GetById(request.SeasonId)
            ?? throw new KeyNotFoundException($"Season {request.SeasonId} not found.");

        season.RemovePlayer(request.PlayerId);
        seasonRepository.Update(season);
    }
}
