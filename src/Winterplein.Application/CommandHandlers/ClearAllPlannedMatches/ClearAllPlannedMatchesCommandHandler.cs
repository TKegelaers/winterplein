using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;

namespace Winterplein.Application.CommandHandlers.ClearAllPlannedMatches;

public static class ClearAllPlannedMatchesCommandHandler
{
    public static async Task Handle(
        ClearAllPlannedMatchesCommand command,
        ISeasonRepository seasonRepository,
        IPlannedMatchRepository plannedMatchRepository,
        CancellationToken ct = default)
    {
        _ = await seasonRepository.GetByIdAsync(command.SeasonId, ct)
            ?? throw new KeyNotFoundException($"Season {command.SeasonId} not found.");

        await plannedMatchRepository.DeleteAllBySeasonAsync(command.SeasonId, ct);
    }
}
