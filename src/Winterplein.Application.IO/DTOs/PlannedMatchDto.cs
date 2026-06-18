namespace Winterplein.Application.IO.DTOs;

public record PlannedMatchDto(int Id, int SeasonId, DateOnly Date, TeamDto Team1, TeamDto Team2);
