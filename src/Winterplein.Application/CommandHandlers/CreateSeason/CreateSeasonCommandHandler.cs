using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.CommandHandlers.CreateSeason;

public static class CreateSeasonCommandHandler
{
    public static async Task<Season> Handle(CreateSeasonCommand command, ISeasonRepository seasonRepository, CancellationToken ct = default)
    {
        var season = new Season(0, command.Name, command.StartDate, command.EndDate,
            command.Weekday, command.StartHour, command.EndHour);
        return await seasonRepository.AddAsync(season, ct);
    }
}
