namespace Winterplein.Application.IO.Commands;

public record CreateSeasonCommand(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    DayOfWeek Weekday,
    TimeOnly StartHour,
    TimeOnly EndHour);
