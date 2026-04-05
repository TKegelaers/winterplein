using MediatR;

namespace Winterplein.Application.Seasons;

public record CreateSeasonCommand(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    DayOfWeek Weekday,
    TimeOnly StartHour,
    TimeOnly EndHour) : IRequest<int>;
