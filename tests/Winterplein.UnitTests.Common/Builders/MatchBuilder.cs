using Winterplein.Domain.Entities;

namespace Winterplein.UnitTests.Common.Builders;

public class MatchBuilder
{
    private int _id = 1;
    private Team _team1 = new TeamBuilder().WithId(1).WithPlayer1(new PlayerBuilder().WithId(1).Build()).WithPlayer2(new PlayerBuilder().WithId(2).Build()).Build();
    private Team _team2 = new TeamBuilder().WithId(2).WithPlayer1(new PlayerBuilder().WithId(3).Build()).WithPlayer2(new PlayerBuilder().WithId(4).Build()).Build();

    public MatchBuilder WithId(int id) { _id = id; return this; }
    public MatchBuilder WithTeam1(Team team1) { _team1 = team1; return this; }
    public MatchBuilder WithTeam2(Team team2) { _team2 = team2; return this; }

    public Match Build() => new(_id, _team1, _team2);
}
