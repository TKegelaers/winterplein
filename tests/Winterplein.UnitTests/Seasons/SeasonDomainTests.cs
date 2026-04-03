using Winterplein.Domain.Entities;
using Winterplein.UnitTests.Common.Builders;

namespace Winterplein.UnitTests.Seasons;

public class SeasonDomainTests
{
    // --- GetMatchdays correctness ---

    [Fact]
    public void GetMatchdays_ReturnsCorrectDates_ForTypicalRange()
    {
        var season = new SeasonBuilder()
            .WithStartDate(new DateOnly(2025, 1, 6))
            .WithEndDate(new DateOnly(2025, 3, 31))
            .WithWeekday(DayOfWeek.Monday)
            .Build();

        var matchdays = season.GetMatchdays();

        matchdays.Should().NotBeEmpty();
        matchdays.Should().AllSatisfy(d => d.DayOfWeek.Should().Be(DayOfWeek.Monday));
        matchdays.First().Should().Be(new DateOnly(2025, 1, 6));
        matchdays.Last().Should().Be(new DateOnly(2025, 3, 31));
    }

    [Fact]
    public void GetMatchdays_ReturnsEmpty_WhenNoWeekdayFallsInRange()
    {
        // Jan 6 is a Monday; a range of Tue–Sun with weekday=Monday returns nothing
        var season = new SeasonBuilder()
            .WithStartDate(new DateOnly(2025, 1, 7))
            .WithEndDate(new DateOnly(2025, 1, 12))
            .WithWeekday(DayOfWeek.Monday)
            .Build();

        season.GetMatchdays().Should().BeEmpty();
    }

    [Fact]
    public void GetMatchdays_IncludesStartAndEndDate_WhenTheyAreTargetWeekday()
    {
        var season = new SeasonBuilder()
            .WithStartDate(new DateOnly(2025, 1, 6))  // Monday
            .WithEndDate(new DateOnly(2025, 1, 13))   // Monday
            .WithWeekday(DayOfWeek.Monday)
            .Build();

        var matchdays = season.GetMatchdays();

        matchdays.Should().Contain(new DateOnly(2025, 1, 6));
        matchdays.Should().Contain(new DateOnly(2025, 1, 13));
        matchdays.Should().HaveCount(2);
    }

    [Fact]
    public void GetMatchdays_ReturnsSingleDate_WhenSingleDayRangeMatchesWeekday()
    {
        var season = new SeasonBuilder()
            .WithStartDate(new DateOnly(2025, 1, 6))  // Monday
            .WithEndDate(new DateOnly(2025, 1, 7))    // Tuesday (> start, so valid season)
            .WithWeekday(DayOfWeek.Monday)
            .Build();

        var matchdays = season.GetMatchdays();

        matchdays.Should().HaveCount(1);
        matchdays[0].Should().Be(new DateOnly(2025, 1, 6));
    }

    // --- Validation ---

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ThrowsArgumentException_WhenNameIsEmptyOrWhitespace(string name)
    {
        var act = () => new Season(1, name,
            new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0));

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenEndDateNotAfterStartDate()
    {
        var act = () => new Season(1, "Test",
            new DateOnly(2025, 6, 1), new DateOnly(2025, 5, 1),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0));

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenEndDateEqualsStartDate()
    {
        var act = () => new Season(1, "Test",
            new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 1),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0));

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenEndHourNotAfterStartHour()
    {
        var act = () => new Season(1, "Test",
            new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(20, 0), new TimeOnly(18, 0));

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenEndHourEqualsStartHour()
    {
        var act = () => new Season(1, "Test",
            new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(18, 0));

        act.Should().Throw<ArgumentException>();
    }

    // --- AddPlayer / RemovePlayer ---

    [Fact]
    public void AddPlayer_AddsPlayerToSeason()
    {
        var season = new SeasonBuilder().Build();
        var player = new PlayerBuilder().WithId(10).Build();

        season.AddPlayer(player);

        season.Players.Should().Contain(p => p.Id == 10);
    }

    [Fact]
    public void AddPlayer_ThrowsArgumentNullException_WhenPlayerIsNull()
    {
        var season = new SeasonBuilder().Build();

        var act = () => season.AddPlayer(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddPlayer_ThrowsArgumentException_WhenPlayerAlreadyEnrolled()
    {
        var player = new PlayerBuilder().WithId(1).Build();
        var season = new SeasonBuilder().WithPlayer(player).Build();

        var act = () => season.AddPlayer(player);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RemovePlayer_RemovesPlayerFromSeason()
    {
        var players = Enumerable.Range(1, 5)
            .Select(i => new PlayerBuilder().WithId(i).Build()).ToList();
        var season = new SeasonBuilder().Build();
        players.ForEach(season.AddPlayer);

        season.RemovePlayer(1);

        season.Players.Should().NotContain(p => p.Id == 1);
    }

    [Fact]
    public void RemovePlayer_ThrowsKeyNotFoundException_WhenPlayerNotEnrolled()
    {
        var season = new SeasonBuilder().Build();

        var act = () => season.RemovePlayer(999);

        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void RemovePlayer_ThrowsInvalidOperationException_WhenExactlyFourPlayersEnrolled()
    {
        var players = Enumerable.Range(1, 4)
            .Select(i => new PlayerBuilder().WithId(i).Build()).ToList();
        var season = new SeasonBuilder().Build();
        players.ForEach(season.AddPlayer);

        var act = () => season.RemovePlayer(1);

        act.Should().Throw<ArgumentException>();
    }
}
