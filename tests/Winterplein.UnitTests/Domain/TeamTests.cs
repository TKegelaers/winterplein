using Winterplein.Domain.Entities;
using Winterplein.UnitTests.Common.Builders;

namespace Winterplein.UnitTests.Domain;

public class TeamTests
{
    [Fact]
    public void Team_AB_Equals_Team_BA()
    {
        var a = new PlayerBuilder().WithId(1).Build();
        var b = new PlayerBuilder().WithId(2).Build();
        var team1 = new Team(1, a, b);
        var team2 = new Team(2, b, a);

        team1.Should().Be(team2);
    }

    [Fact]
    public void Team_AB_DoesNotEqual_Team_AC()
    {
        var a = new PlayerBuilder().WithId(1).Build();
        var b = new PlayerBuilder().WithId(2).Build();
        var c = new PlayerBuilder().WithId(3).Build();
        var team1 = new Team(1, a, b);
        var team2 = new Team(2, a, c);

        team1.Should().NotBe(team2);
    }

    [Fact]
    public void Team_HashCode_ConsistentWithEquality()
    {
        var a = new PlayerBuilder().WithId(1).Build();
        var b = new PlayerBuilder().WithId(2).Build();
        var team1 = new Team(1, a, b);
        var team2 = new Team(2, b, a);

        team1.GetHashCode().Should().Be(team2.GetHashCode());
    }

    [Fact]
    public void Team_StoresPlayer1AndPlayer2()
    {
        var a = new PlayerBuilder().WithId(1).Build();
        var b = new PlayerBuilder().WithId(2).Build();
        var team = new Team(1, a, b);

        team.Player1.Should().Be(a);
        team.Player2.Should().Be(b);
    }

    [Fact]
    public void Team_Constructor_ThrowsWhenPlayer1IsNull()
    {
        var b = new PlayerBuilder().WithId(2).Build();
        var act = () => new Team(1, null!, b);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Team_Constructor_ThrowsWhenPlayer2IsNull()
    {
        var a = new PlayerBuilder().WithId(1).Build();
        var act = () => new Team(1, a, null!);
        act.Should().Throw<ArgumentNullException>();
    }
}
