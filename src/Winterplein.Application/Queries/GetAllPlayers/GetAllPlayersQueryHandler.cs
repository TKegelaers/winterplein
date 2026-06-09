using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Queries.GetAllPlayers;

public static class GetAllPlayersQueryHandler
{
    public static async Task<List<PlayerDto>> Handle(GetAllPlayersQuery query, IPlayerRepository repo, CancellationToken ct = default) =>
        (await repo.GetAllAsync(ct)).Select(p => p.ToDto()).ToList();
}
