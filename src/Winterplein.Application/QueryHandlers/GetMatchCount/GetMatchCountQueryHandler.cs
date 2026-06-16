using Winterplein.Application.Ports;
using Winterplein.Application.IO.DTOs;
using Winterplein.Application.IO.Queries;

namespace Winterplein.Application.QueryHandlers.GetMatchCount;

public static class GetMatchCountQueryHandler
{
    public static async Task<MatchCountResponse> Handle(GetMatchCountQuery query, IPlayerRepository repo, IMatchGeneratorService generator, CancellationToken ct = default) =>
        new MatchCountResponse(generator.CalculateMatchCount(await repo.CountAsync(ct)));
}
