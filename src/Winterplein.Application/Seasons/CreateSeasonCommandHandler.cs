using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public static class CreateSeasonCommandHandler
{
    public static Season Handle(CreateSeasonCommand command, ISeasonRepository seasonRepository)
    {
        var season = new Season(0, command.Name, command.StartDate, command.EndDate,
            command.Weekday, command.StartHour, command.EndHour);
        return seasonRepository.Add(season);
    }
}
