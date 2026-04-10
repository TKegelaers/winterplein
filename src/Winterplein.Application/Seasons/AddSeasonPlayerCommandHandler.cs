using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Seasons;

public static class AddSeasonPlayerCommandHandler
{
    public static SeasonDto? Handle(AddSeasonPlayerCommand request, ISeasonRepository seasonRepository, IPlayerRepository playerRepository)
    {
        var season = seasonRepository.GetById(request.SeasonId);
        if (season == null)
            return null;

        var player = playerRepository.GetById(request.PlayerId);
        if (player == null)
            return null;

        season.AddPlayer(player);
        seasonRepository.Update(season);
        return season.ToDto();
    }
}
