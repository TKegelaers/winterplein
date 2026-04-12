using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public static class UpdateSeasonCommandHandler
{
    public static Season Handle(UpdateSeasonCommand command, ISeasonRepository seasonRepository)
    {
        var existing = seasonRepository.GetById(command.Id)
            ?? throw new KeyNotFoundException($"Season {command.Id} not found.");

        var updated = new Season(command.Id, command.Name, command.StartDate, command.EndDate,
            command.Weekday, command.StartHour, command.EndHour, existing.Players);
        seasonRepository.Update(updated);
        return updated;
    }
}
