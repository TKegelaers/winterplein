using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;

namespace Winterplein.Application.CommandHandlers.ClearPlannedMatch;

public static class ClearPlannedMatchCommandHandler
{
    public static async Task Handle(
        ClearPlannedMatchCommand command,
        ISeasonRepository seasonRepository,
        IPlannedMatchRepository plannedMatchRepository,
        CancellationToken ct = default)
    {
        _ = await seasonRepository.GetByIdAsync(command.SeasonId, ct)
            ?? throw new KeyNotFoundException($"Season {command.SeasonId} not found.");

        var deleted = await plannedMatchRepository.DeleteBySeasonAndDateAsync(command.SeasonId, command.Date, ct);
        if (!deleted)
            throw new KeyNotFoundException($"No planned match for season {command.SeasonId} on {command.Date:O}.");
    }
}
