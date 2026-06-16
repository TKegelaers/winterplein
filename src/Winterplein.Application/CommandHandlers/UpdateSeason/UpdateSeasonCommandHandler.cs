using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.CommandHandlers.UpdateSeason;

public static class UpdateSeasonCommandHandler
{
    public static async Task<Season> Handle(UpdateSeasonCommand command, ISeasonRepository seasonRepository, CancellationToken ct = default)
    {
        var existing = await seasonRepository.GetByIdAsync(command.Id, ct)
            ?? throw new KeyNotFoundException($"Season {command.Id} not found.");

        var updated = new Season(command.Id, command.Name, command.StartDate, command.EndDate,
            command.Weekday, command.StartHour, command.EndHour, existing.Players);
        return await seasonRepository.UpdateAsync(updated, ct);
    }
}
