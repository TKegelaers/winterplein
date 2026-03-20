using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;

namespace Winterplein.UnitTests.Domain;

public class TeamTests
{
    private static Player MakePlayer(int id) =>
        new(id, new Name("Test", "Player")) { Gender = Gender.Male };

    [Fact]
    public void Team_AB_Equals_Team_BA()
    {
        var a = MakePlayer(1);
        var b = MakePlayer(2);
        var team1 = new Team(1, a, b);
        var team2 = new Team(2, b, a);

        Assert.Equal(team1, team2);
    }

    [Fact]
    public void Team_AB_DoesNotEqual_Team_AC()
    {
        var a = MakePlayer(1);
        var b = MakePlayer(2);
        var c = MakePlayer(3);
        var team1 = new Team(1, a, b);
        var team2 = new Team(2, a, c);

        Assert.NotEqual(team1, team2);
    }

    [Fact]
    public void Team_HashCode_ConsistentWithEquality()
    {
        var a = MakePlayer(1);
        var b = MakePlayer(2);
        var team1 = new Team(1, a, b);
        var team2 = new Team(2, b, a);

        Assert.Equal(team1.GetHashCode(), team2.GetHashCode());
    }

    [Fact]
    public void Team_StoresPlayer1AndPlayer2()
    {
        var a = MakePlayer(1);
        var b = MakePlayer(2);
        var team = new Team(1, a, b);

        Assert.Equal(a, team.Player1);
        Assert.Equal(b, team.Player2);
    }

    [Fact]
    public void Team_Constructor_ThrowsWhenPlayer1IsNull()
    {
        var b = MakePlayer(2);
        Assert.Throws<ArgumentNullException>(() => new Team(1, null!, b));
    }

    [Fact]
    public void Team_Constructor_ThrowsWhenPlayer2IsNull()
    {
        var a = MakePlayer(1);
        Assert.Throws<ArgumentNullException>(() => new Team(1, a, null!));
    }
}
