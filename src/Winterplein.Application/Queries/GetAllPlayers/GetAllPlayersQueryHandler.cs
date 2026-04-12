using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Queries.GetAllPlayers;

public static class GetAllPlayersQueryHandler
{
    public static List<PlayerDto> Handle(GetAllPlayersQuery query, IPlayerRepository repo) =>
        repo.GetAll().Select(p => p.ToDto()).ToList();
}
