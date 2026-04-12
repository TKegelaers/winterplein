using Winterplein.Application.Interfaces;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Queries.GetMatchCount;

public static class GetMatchCountQueryHandler
{
    public static MatchCountResponse Handle(GetMatchCountQuery query, IPlayerRepository repo, IMatchGeneratorService generator) =>
        new MatchCountResponse(generator.CalculateMatchCount(repo.Count));
}
