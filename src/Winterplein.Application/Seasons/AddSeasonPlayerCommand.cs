using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Seasons;

public record AddSeasonPlayerCommand(int SeasonId, int PlayerId);
