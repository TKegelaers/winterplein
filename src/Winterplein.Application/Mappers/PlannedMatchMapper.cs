using Winterplein.Domain.Entities;
using Winterplein.Application.IO.DTOs;

namespace Winterplein.Application.Mappers;

public static class PlannedMatchMapper
{
    public static PlannedMatch ToSnapshot(this Match match, int seasonId, DateOnly date)
        => new(0, seasonId, date, match.Team1.ToSnapshot(), match.Team2.ToSnapshot());

    public static PlannedTeam ToSnapshot(this Team team)
        => new(team.Player1.ToSnapshot(), team.Player2.ToSnapshot());

    public static PlannedPlayer ToSnapshot(this Player player)
        => new(player.Id, player.Name.FirstName, player.Name.LastName, player.Gender);

    public static PlannedMatchDto ToDto(this PlannedMatch plannedMatch)
        => new(plannedMatch.Id, plannedMatch.SeasonId, plannedMatch.Date,
            plannedMatch.Team1.ToDto(), plannedMatch.Team2.ToDto());

    public static TeamDto ToDto(this PlannedTeam team)
        => new(0, team.Player1.ToDto(), team.Player2.ToDto());

    public static PlayerDto ToDto(this PlannedPlayer player)
        => new(player.PlayerId, player.FirstName, player.LastName, player.Gender.ToString());
}
