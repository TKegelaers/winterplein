using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public static class CreateSeasonCommandHandler
{
    public static Season Handle(CreateSeasonCommand request, ISeasonRepository seasonRepository)
    {
        var season = new Season(0, request.Name, request.StartDate, request.EndDate,
            request.Weekday, request.StartHour, request.EndHour);
        return seasonRepository.Add(season);
    }
}
