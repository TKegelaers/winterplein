using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;

namespace Winterplein.Domain.UnitTests.Domain;

public class PlannedMatchTests
{
    private static PlannedTeam BuildTeam(int player1Id = 1, int player2Id = 2) =>
        new(
            new PlannedPlayer(player1Id, "John", "Doe", Gender.Male),
            new PlannedPlayer(player2Id, "Jane", "Roe", Gender.Female));

    [Fact]
    public void Constructs_WithValidSnapshotAndDate()
    {
        var date = new DateOnly(2026, 1, 7);
        var team1 = BuildTeam(1, 2);
        var team2 = BuildTeam(3, 4);

        var match = new PlannedMatch(10, 5, date, team1, team2);

        match.Id.Should().Be(10);
        match.SeasonId.Should().Be(5);
        match.Date.Should().Be(date);
        match.Team1.Should().BeSameAs(team1);
        match.Team2.Should().BeSameAs(team2);
    }

    [Fact]
    public void Throws_ForDefaultDate()
    {
        var act = () => new PlannedMatch(1, 1, default, BuildTeam(1, 2), BuildTeam(3, 4));

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Snapshot_ExposesPlayerIdNameAndGender()
    {
        var date = new DateOnly(2026, 1, 7);
        var team1 = new PlannedTeam(
            new PlannedPlayer(7, "Alice", "Smith", Gender.Female),
            new PlannedPlayer(8, "Bob", "Jones", Gender.Male));

        var match = new PlannedMatch(1, 1, date, team1, BuildTeam(3, 4));

        match.Team1.Player1.PlayerId.Should().Be(7);
        match.Team1.Player1.FirstName.Should().Be("Alice");
        match.Team1.Player1.LastName.Should().Be("Smith");
        match.Team1.Player1.Gender.Should().Be(Gender.Female);
        match.Team1.Player2.PlayerId.Should().Be(8);
        match.Team1.Player2.Gender.Should().Be(Gender.Male);
    }
}
