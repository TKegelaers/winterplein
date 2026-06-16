namespace Winterplein.Application.IO.DTOs;

public record UpdateSeasonRequest(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    DayOfWeek Weekday,
    TimeOnly StartHour,
    TimeOnly EndHour);
