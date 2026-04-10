using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Seasons;

public static class GetSeasonPlayersQueryHandler
{
    public static List<PlayerDto>? Handle(GetSeasonPlayersQuery query, ISeasonRepository seasonRepository)
    {
        var season = seasonRepository.GetById(query.SeasonId);
        if (season == null)
            return null;

        return season.Players.Select(p => p.ToDto()).ToList();
    }
}
