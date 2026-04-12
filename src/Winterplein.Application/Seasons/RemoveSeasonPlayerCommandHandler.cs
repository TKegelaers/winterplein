using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Seasons;

public static class RemoveSeasonPlayerCommandHandler
{
    public static void Handle(RemoveSeasonPlayerCommand command, ISeasonRepository seasonRepository)
    {
        var season = seasonRepository.GetById(command.SeasonId)
            ?? throw new KeyNotFoundException($"Season {command.SeasonId} not found.");

        season.RemovePlayer(command.PlayerId);
        seasonRepository.Update(season);
    }
}
