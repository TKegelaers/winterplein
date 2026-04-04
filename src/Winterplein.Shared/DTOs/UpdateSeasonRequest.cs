namespace Winterplein.Shared.DTOs;

public record UpdateSeasonRequest(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    DayOfWeek Weekday,
    TimeOnly StartHour,
    TimeOnly EndHour);
