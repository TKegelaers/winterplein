using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Seasons;

public static class RemoveSeasonPlayerCommandHandler
{
    public static async Task Handle(RemoveSeasonPlayerCommand command, ISeasonRepository seasonRepository, CancellationToken ct = default)
    {
        var season = await seasonRepository.GetByIdAsync(command.SeasonId, ct)
            ?? throw new KeyNotFoundException($"Season {command.SeasonId} not found.");

        season.RemovePlayer(command.PlayerId);
        await seasonRepository.UpdateAsync(season, ct);
    }
}
