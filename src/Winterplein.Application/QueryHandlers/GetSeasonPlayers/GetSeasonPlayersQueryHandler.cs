using Winterplein.Application.Ports;
using Winterplein.Application.IO.Queries;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.QueryHandlers.GetSeasonPlayers;

public static class GetSeasonPlayersQueryHandler
{
    public static async Task<List<Player>?> Handle(GetSeasonPlayersQuery query, ISeasonRepository seasonRepository, CancellationToken ct = default)
    {
        var season = await seasonRepository.GetByIdAsync(query.SeasonId, ct);
        if (season == null)
            return null;

        return season.Players.ToList();
    }
}
