using Winterplein.Application.Services;
using Winterplein.UnitTests.Common.Builders;

namespace Winterplein.UnitTests.Application;

public class MatchGeneratorServiceTests
{
    private readonly MatchGeneratorService _sut = new();

    private static List<Winterplein.Domain.Entities.Player> BuildPlayers(int count)
        => Enumerable.Range(1, count)
            .Select(i => new PlayerBuilder().WithId(i).Build())
            .ToList();

    [Theory]
    [InlineData(3, 0)]
    [InlineData(4, 3)]
    [InlineData(6, 45)]
    [InlineData(8, 210)]
    [InlineData(10, 630)]
    public void GenerateAllMatches_ReturnsExpectedCount(int playerCount, int expectedMatches)
    {
        var players = BuildPlayers(playerCount);

        var result = _sut.GenerateAllMatches(players);

        result.Should().HaveCount(expectedMatches);
    }

    [Theory]
    [InlineData(3, 0)]
    [InlineData(4, 3)]
    [InlineData(6, 45)]
    [InlineData(8, 210)]
    [InlineData(10, 630)]
    public void CalculateMatchCount_ReturnsExpectedCount(int playerCount, int expectedMatches)
    {
        _sut.CalculateMatchCount(playerCount).Should().Be(expectedMatches);
    }

    [Fact]
    public void GenerateAllMatches_MatchNumbersAreUniqueAndStartAtOne()
    {
        var players = BuildPlayers(6);

        var result = _sut.GenerateAllMatches(players);

        var ids = result.Select(m => m.Id).ToList();
        ids.Should().StartWith([1]);
        ids.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void GenerateAllMatches_NoDuplicateMatches()
    {
        var players = BuildPlayers(6);

        var result = _sut.GenerateAllMatches(players);

        // A match is a duplicate if it has the same two teams (ignoring order of teams and order within team)
        var matchKeys = result.Select(m =>
        {
            var t1 = new HashSet<int> { m.Team1.Player1.Id, m.Team1.Player2.Id };
            var t2 = new HashSet<int> { m.Team2.Player1.Id, m.Team2.Player2.Id };
            var ordered = new[] { t1, t2 }.OrderBy(s => s.Min()).ThenBy(s => s.Max()).ToList();
            return string.Join("|", ordered.Select(s => string.Join(",", s.OrderBy(x => x))));
        }).ToList();

        matchKeys.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void GenerateAllMatches_ReturnsEmptyList_WhenFewerThanFourPlayers()
    {
        var players = BuildPlayers(3);

        var result = _sut.GenerateAllMatches(players);

        result.Should().BeEmpty();
    }
}
