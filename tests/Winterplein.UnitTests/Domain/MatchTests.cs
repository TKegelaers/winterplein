using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;

namespace Winterplein.UnitTests.Domain;

public class MatchTests
{
    private static Player MakePlayer(int id) =>
        new(id, new Name("Test", "Player")) { Gender = Gender.Male };

    private static Team MakeTeam(int id, int p1Id, int p2Id) =>
        new(id, MakePlayer(p1Id), MakePlayer(p2Id));

    [Fact]
    public void Match_StoresProperties()
    {
        var team1 = MakeTeam(1, 1, 2);
        var team2 = MakeTeam(2, 3, 4);
        var match = new Match(1, team1, team2);

        Assert.Equal(1, match.Id);
        Assert.Equal(team1, match.Team1);
        Assert.Equal(team2, match.Team2);
    }

    [Fact]
    public void Match_Constructor_ThrowsWhenTeam1IsNull()
    {
        var team2 = MakeTeam(2, 3, 4);
        Assert.Throws<ArgumentNullException>(() => new Match(1, null!, team2));
    }

    [Fact]
    public void Match_Constructor_ThrowsWhenTeam2IsNull()
    {
        var team1 = MakeTeam(1, 1, 2);
        Assert.Throws<ArgumentNullException>(() => new Match(1, team1, null!));
    }
}
