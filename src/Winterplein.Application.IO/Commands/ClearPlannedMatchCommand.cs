namespace Winterplein.Application.IO.Commands;

public record ClearPlannedMatchCommand(int SeasonId, DateOnly Date);
