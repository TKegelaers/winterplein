using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public static class AddSeasonPlayerCommandHandler
{
    public static Season Handle(AddSeasonPlayerCommand command, ISeasonRepository seasonRepository, IPlayerRepository playerRepository)
    {
        var season = seasonRepository.GetById(command.SeasonId)
            ?? throw new KeyNotFoundException($"Season {command.SeasonId} not found.");

        var player = playerRepository.GetById(command.PlayerId)
            ?? throw new KeyNotFoundException($"Player {command.PlayerId} not found.");

        season.AddPlayer(player);
        seasonRepository.Update(season);
        return season;
    }
}
