using MediatR;
using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Seasons;

public class GetSeasonPlayersQueryHandler(ISeasonRepository seasonRepository)
    : IRequestHandler<GetSeasonPlayersQuery, List<PlayerDto>?>
{
    public Task<List<PlayerDto>?> Handle(GetSeasonPlayersQuery request, CancellationToken cancellationToken)
    {
        var season = seasonRepository.GetById(request.SeasonId);
        if (season == null)
            return Task.FromResult<List<PlayerDto>?>(null);

        return Task.FromResult<List<PlayerDto>?>(
            season.Players.Select(p => p.ToDto()).ToList());
    }
}
