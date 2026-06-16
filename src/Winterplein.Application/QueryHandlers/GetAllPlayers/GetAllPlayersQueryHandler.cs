using Winterplein.Application.Ports;
using Winterplein.Application.Mappers;
using Winterplein.Application.IO.DTOs;
using Winterplein.Application.IO.Queries;

namespace Winterplein.Application.QueryHandlers.GetAllPlayers;

public static class GetAllPlayersQueryHandler
{
    public static async Task<List<PlayerDto>> Handle(GetAllPlayersQuery query, IPlayerRepository repo, CancellationToken ct = default) =>
        (await repo.GetAllAsync(ct)).Select(p => p.ToDto()).ToList();
}
