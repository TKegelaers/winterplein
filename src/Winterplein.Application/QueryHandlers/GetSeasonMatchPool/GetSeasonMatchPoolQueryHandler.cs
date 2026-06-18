using Winterplein.Application.Ports;
using Winterplein.Application.Mappers;
using Winterplein.Application.IO.Queries;
using Winterplein.Application.IO.DTOs;

namespace Winterplein.Application.QueryHandlers.GetSeasonMatchPool;

public static class GetSeasonMatchPoolQueryHandler
{
    public static async Task<GenerateMatchesResponse?> Handle(GetSeasonMatchPoolQuery query, ISeasonRepository seasonRepository, IMatchGeneratorService generator, CancellationToken ct = default)
    {
        var season = await seasonRepository.GetByIdAsync(query.SeasonId, ct);
        if (season == null)
            return null;

        if (season.Players.Count < 4)
            return new GenerateMatchesResponse([], 0);

        var matches = generator.GenerateAllMatches(season.Players);
        return matches.ToResponse();
    }
}
