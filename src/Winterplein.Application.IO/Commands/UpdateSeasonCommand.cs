namespace Winterplein.Application.IO.Commands;

public record UpdateSeasonCommand(
    int Id,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    DayOfWeek Weekday,
    TimeOnly StartHour,
    TimeOnly EndHour);
