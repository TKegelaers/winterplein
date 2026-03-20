using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;

namespace Winterplein.UnitTests.Domain;

public class TeamTests
{
    private static Player MakePlayer(int id) =>
        new(new Name("Test", "Player")) { Id = id, Gender = Gender.Male };

    [Fact]
    public void Team_AB_Equals_Team_BA()
    {
        var a = MakePlayer(1);
        var b = MakePlayer(2);
        var team1 = new Team(a, b) { Id = 1 };
        var team2 = new Team(b, a) { Id = 2 };

        Assert.Equal(team1, team2);
    }

    [Fact]
    public void Team_AB_DoesNotEqual_Team_AC()
    {
        var a = MakePlayer(1);
        var b = MakePlayer(2);
        var c = MakePlayer(3);
        var team1 = new Team(a, b) { Id = 1 };
        var team2 = new Team(a, c) { Id = 2 };

        Assert.NotEqual(team1, team2);
    }

    [Fact]
    public void Team_HashCode_ConsistentWithEquality()
    {
        var a = MakePlayer(1);
        var b = MakePlayer(2);
        var team1 = new Team(a, b) { Id = 1 };
        var team2 = new Team(b, a) { Id = 2 };

        Assert.Equal(team1.GetHashCode(), team2.GetHashCode());
    }

    [Fact]
    public void Team_StoresPlayer1AndPlayer2()
    {
        var a = MakePlayer(1);
        var b = MakePlayer(2);
        var team = new Team(a, b) { Id = 1 };

        Assert.Equal(a, team.Player1);
        Assert.Equal(b, team.Player2);
    }

    [Fact]
    public void Team_Constructor_ThrowsWhenPlayer1IsNull()
    {
        var b = MakePlayer(2);
        Assert.Throws<ArgumentNullException>(() => new Team(null!, b));
    }

    [Fact]
    public void Team_Constructor_ThrowsWhenPlayer2IsNull()
    {
        var a = MakePlayer(1);
        Assert.Throws<ArgumentNullException>(() => new Team(a, null!));
    }
}
