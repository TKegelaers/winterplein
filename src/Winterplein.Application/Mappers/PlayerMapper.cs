using Winterplein.Domain.Entities;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Mappers;

public static class PlayerMapper
{
    public static PlayerDto ToDto(this Player player)
        => new(player.Id, player.Name.FirstName, player.Name.LastName, player.Gender.ToString());
}
