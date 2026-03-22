using Winterplein.Domain.Entities;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Mappers;

public static class MatchMapper
{
    public static MatchDto ToDto(this Match match)
        => new(match.Id, match.Team1.ToDto(), match.Team2.ToDto());
}
