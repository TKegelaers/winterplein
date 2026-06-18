namespace Winterplein.Application.IO.DTOs;

public record MatchdayScheduleEntryDto(DateOnly Date, PlannedMatchDto? PlannedMatch, bool IsPlanned);
