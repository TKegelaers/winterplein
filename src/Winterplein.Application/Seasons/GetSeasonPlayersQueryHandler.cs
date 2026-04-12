using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public static class GetSeasonPlayersQueryHandler
{
    public static List<Player>? Handle(GetSeasonPlayersQuery query, ISeasonRepository seasonRepository)
    {
        var season = seasonRepository.GetById(query.SeasonId);
        if (season == null)
            return null;

        return season.Players.ToList();
    }
}
