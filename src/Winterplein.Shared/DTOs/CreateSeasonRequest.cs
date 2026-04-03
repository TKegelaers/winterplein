namespace Winterplein.Shared.DTOs;

public record CreateSeasonRequest(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    DayOfWeek Weekday,
    TimeOnly StartHour,
    TimeOnly EndHour);
