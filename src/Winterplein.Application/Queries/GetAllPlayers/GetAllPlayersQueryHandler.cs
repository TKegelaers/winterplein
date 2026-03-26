using MediatR;
using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Queries.GetAllPlayers;

public class GetAllPlayersQueryHandler(IPlayerRepository repo)
    : IRequestHandler<GetAllPlayersQuery, List<PlayerDto>>
{
    public Task<List<PlayerDto>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(repo.GetAll().Select(p => p.ToDto()).ToList());
}
