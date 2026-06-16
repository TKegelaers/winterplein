using Winterplein.Application.Interfaces;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Queries.GetMatchCount;

public static class GetMatchCountQueryHandler
{
    public static async Task<MatchCountResponse> Handle(GetMatchCountQuery query, IPlayerRepository repo, IMatchGeneratorService generator, CancellationToken ct = default) =>
        new MatchCountResponse(generator.CalculateMatchCount(await repo.CountAsync(ct)));
}
