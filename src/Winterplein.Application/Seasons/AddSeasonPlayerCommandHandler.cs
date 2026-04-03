using MediatR;
using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Seasons;

public class AddSeasonPlayerCommandHandler(ISeasonRepository seasonRepository, IPlayerRepository playerRepository)
    : IRequestHandler<AddSeasonPlayerCommand, SeasonDto?>
{
    public Task<SeasonDto?> Handle(AddSeasonPlayerCommand request, CancellationToken cancellationToken)
    {
        var season = seasonRepository.GetById(request.SeasonId);
        if (season == null)
            return Task.FromResult<SeasonDto?>(null);

        var player = playerRepository.GetById(request.PlayerId);
        if (player == null)
            return Task.FromResult<SeasonDto?>(null);

        season.AddPlayer(player);
        seasonRepository.Update(season);
        return Task.FromResult<SeasonDto?>(season.ToDto());
    }
}
