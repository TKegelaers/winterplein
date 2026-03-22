using Winterplein.Domain.Entities;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Mappers;

public static class TeamMapper
{
    public static TeamDto ToDto(this Team team)
        => new(team.Id, team.Player1.ToDto(), team.Player2.ToDto());
}
