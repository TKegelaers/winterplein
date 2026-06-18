namespace Winterplein.Application.IO.DTOs;

public record GenerateScheduleResponse(List<PlannedMatchDto> PlannedMatches, int PlannedCount, int OpenCount);
