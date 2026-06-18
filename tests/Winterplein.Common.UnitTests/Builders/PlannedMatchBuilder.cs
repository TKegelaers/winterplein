using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;

namespace Winterplein.Common.UnitTests.Builders;

public class PlannedMatchBuilder
{
    private int _id = 1;
    private int _seasonId = 1;
    private DateOnly _date = new(2025, 1, 6);
    private PlannedTeam _team1 = new(
        new PlannedPlayer(1, "Alice", "Alpha", Gender.Female),
        new PlannedPlayer(2, "Bob", "Beta", Gender.Male));
    private PlannedTeam _team2 = new(
        new PlannedPlayer(3, "Carol", "Gamma", Gender.Female),
        new PlannedPlayer(4, "Dave", "Delta", Gender.Male));

    public PlannedMatchBuilder WithId(int id) { _id = id; return this; }
    public PlannedMatchBuilder WithSeasonId(int seasonId) { _seasonId = seasonId; return this; }
    public PlannedMatchBuilder WithDate(DateOnly date) { _date = date; return this; }
    public PlannedMatchBuilder WithTeam1(PlannedTeam team1) { _team1 = team1; return this; }
    public PlannedMatchBuilder WithTeam2(PlannedTeam team2) { _team2 = team2; return this; }

    public PlannedMatchBuilder WithPlayers(int p1, int p2, int p3, int p4)
    {
        _team1 = new PlannedTeam(
            new PlannedPlayer(p1, "Player", $"P{p1}", Gender.Male),
            new PlannedPlayer(p2, "Player", $"P{p2}", Gender.Male));
        _team2 = new PlannedTeam(
            new PlannedPlayer(p3, "Player", $"P{p3}", Gender.Male),
            new PlannedPlayer(p4, "Player", $"P{p4}", Gender.Male));
        return this;
    }

    public PlannedMatch Build() => new(_id, _seasonId, _date, _team1, _team2);
}
