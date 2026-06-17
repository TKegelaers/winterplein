using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.CommandHandlers.AddSeasonPlayer;

public static class AddSeasonPlayerCommandHandler
{
    public static async Task<Season> Handle(AddSeasonPlayerCommand command, ISeasonRepository seasonRepository, IPlayerRepository playerRepository, CancellationToken ct = default)
    {
        var season = await seasonRepository.GetByIdAsync(command.SeasonId, ct)
            ?? throw new KeyNotFoundException($"Season {command.SeasonId} not found.");

        var player = await playerRepository.GetByIdAsync(command.PlayerId, ct)
            ?? throw new KeyNotFoundException($"Player {command.PlayerId} not found.");

        season.AddPlayer(player);
        return await seasonRepository.UpdateAsync(season, ct);
    }
}
