using Winterplein.Domain.Entities;
using Winterplein.UnitTests.Common.Builders;

namespace Winterplein.UnitTests.Domain;

public class MatchTests
{
    [Fact]
    public void Match_StoresProperties()
    {
        var team1 = new TeamBuilder().WithId(1).WithPlayer1(new PlayerBuilder().WithId(1).Build()).WithPlayer2(new PlayerBuilder().WithId(2).Build()).Build();
        var team2 = new TeamBuilder().WithId(2).WithPlayer1(new PlayerBuilder().WithId(3).Build()).WithPlayer2(new PlayerBuilder().WithId(4).Build()).Build();
        var match = new Match(1, team1, team2);

        match.Id.Should().Be(1);
        match.Team1.Should().Be(team1);
        match.Team2.Should().Be(team2);
    }

    [Fact]
    public void Match_Constructor_ThrowsWhenTeam1IsNull()
    {
        var team2 = new TeamBuilder().WithId(2).WithPlayer1(new PlayerBuilder().WithId(3).Build()).WithPlayer2(new PlayerBuilder().WithId(4).Build()).Build();
        var act = () => new Match(1, null!, team2);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Match_Constructor_ThrowsWhenTeam2IsNull()
    {
        var team1 = new TeamBuilder().WithId(1).WithPlayer1(new PlayerBuilder().WithId(1).Build()).WithPlayer2(new PlayerBuilder().WithId(2).Build()).Build();
        var act = () => new Match(1, team1, null!);
        act.Should().Throw<ArgumentNullException>();
    }
}
