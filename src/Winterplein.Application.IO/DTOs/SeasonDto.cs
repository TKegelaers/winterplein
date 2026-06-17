namespace Winterplein.Application.IO.DTOs;

public record SeasonDto(
    int Id,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    DayOfWeek Weekday,
    TimeOnly StartHour,
    TimeOnly EndHour,
    List<DateOnly> Matchdays,
    int MatchdayCount,
    List<PlayerDto> Players);
