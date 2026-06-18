using Moq;
using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;
using Winterplein.Application.CommandHandlers.GenerateSchedule;
using Winterplein.Application.Services;
using Winterplein.Domain.Entities;
using Winterplein.Common.UnitTests.Builders;

namespace Winterplein.Application.UnitTests.Seasons;

public class GenerateScheduleHandlerTests
{
    private readonly Mock<ISeasonRepository> _seasonRepo = new();
    private readonly Mock<IPlannedMatchRepository> _plannedRepo = new();
    private readonly MatchGeneratorService _generator = new();
    private readonly List<PlannedMatch> _persisted = [];

    public GenerateScheduleHandlerTests()
    {
        _plannedRepo
            .Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<PlannedMatch>>(), It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<PlannedMatch>, CancellationToken>((m, _) => _persisted.AddRange(m))
            .Returns(Task.CompletedTask);
    }

    private void SetupSeason(Season season)
        => _seasonRepo.Setup(r => r.GetByIdAsync(season.Id, It.IsAny<CancellationToken>())).ReturnsAsync(season);

    private void SetupExisting(int seasonId, params PlannedMatch[] existing)
        => _plannedRepo.Setup(r => r.GetAllBySeasonAsync(seasonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing.ToList());

    // A season with 6 enrolled players (pool of 45 matches) and a small set of matchdays.
    private static Season SixPlayerSeason(int id = 1)
    {
        var builder = new SeasonBuilder()
            .WithId(id)
            .WithStartDate(new DateOnly(2025, 1, 6))   // Monday
            .WithEndDate(new DateOnly(2025, 1, 27))    // 4 Mondays: 6, 13, 20, 27
            .WithWeekday(DayOfWeek.Monday);
        for (var i = 1; i <= 6; i++)
            builder.WithPlayer(new PlayerBuilder().WithId(i).Build());
        return builder.Build();
    }

    private static (long, long) Composition(PlannedMatch m)
    {
        long Encode(int x, int y) => ((long)Math.Min(x, y) << 32) | (uint)Math.Max(x, y);
        var t1 = Encode(m.Team1.Player1.PlayerId, m.Team1.Player2.PlayerId);
        var t2 = Encode(m.Team2.Player1.PlayerId, m.Team2.Player2.PlayerId);
        return t1 <= t2 ? (t1, t2) : (t2, t1);
    }

    [Fact]
    public async Task FillsEveryOpenMatchday_WithUniqueMatch()
    {
        var season = SixPlayerSeason();
        SetupSeason(season);
        SetupExisting(season.Id);

        var result = await GenerateScheduleHandler_Handle(season.Id);

        var matchdays = season.GetMatchdays();
        result.Should().NotBeNull();
        result!.OpenCount.Should().Be(0);
        result.PlannedCount.Should().Be(matchdays.Count);
        result.PlannedMatches.Should().HaveCount(matchdays.Count);
        result.PlannedMatches.Select(p => p.Date).Should().BeEquivalentTo(matchdays);
    }

    [Fact]
    public async Task SkipsAlreadyPlannedMatchdays_OnRerun()
    {
        var season = SixPlayerSeason();
        var matchdays = season.GetMatchdays();
        var alreadyPlanned = new PlannedMatchBuilder()
            .WithId(99)
            .WithSeasonId(season.Id)
            .WithDate(matchdays[0])
            .WithPlayers(1, 2, 3, 4)
            .Build();

        SetupSeason(season);
        SetupExisting(season.Id, alreadyPlanned);

        var result = await GenerateScheduleHandler_Handle(season.Id);

        // Existing planned matchday is untouched; only the remaining 3 are filled.
        _persisted.Should().HaveCount(matchdays.Count - 1);
        _persisted.Select(p => p.Date).Should().NotContain(matchdays[0]);
        result!.OpenCount.Should().Be(0);
        result.PlannedCount.Should().Be(matchdays.Count);
        result.PlannedMatches.Should().Contain(p => p.Id == 99 && p.Date == matchdays[0]);
    }

    [Fact]
    public async Task PartialFill_WhenPoolSmallerThanOpenMatchdays_SetsOpenCount()
    {
        var season = SixPlayerSeason();
        var matchdays = season.GetMatchdays(); // 4 matchdays
        SetupSeason(season);
        SetupExisting(season.Id);

        // Mock a pool with only 1 match -> only 1 matchday can be filled.
        var smallGenerator = new Mock<IMatchGeneratorService>();
        smallGenerator
            .Setup(g => g.GenerateAllMatches(It.IsAny<IReadOnlyList<Player>>()))
            .Returns([new MatchBuilder().WithId(1).Build()]);

        var result = await GenerateScheduleCommandHandler.Handle(
            new GenerateScheduleCommand(season.Id), _seasonRepo.Object, smallGenerator.Object, _plannedRepo.Object);

        result.Should().NotBeNull();
        result!.PlannedCount.Should().Be(1);
        result.OpenCount.Should().Be(matchdays.Count - 1);
        result.PlannedMatches.Should().HaveCount(1);
    }

    [Fact]
    public async Task SeasonWideUniqueness_NoCompositionAssignedTwice()
    {
        var season = SixPlayerSeason();
        SetupSeason(season);
        SetupExisting(season.Id);

        var result = await GenerateScheduleHandler_Handle(season.Id);

        result.Should().NotBeNull();
        var keys = _persisted.Select(Composition).ToList();
        keys.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task SeasonWideUniqueness_ExcludesAlreadyPlannedComposition()
    {
        // Season with exactly 4 players -> pool of exactly 3 unique matches.
        var builder = new SeasonBuilder()
            .WithId(1)
            .WithStartDate(new DateOnly(2025, 1, 6))
            .WithEndDate(new DateOnly(2025, 1, 27))
            .WithWeekday(DayOfWeek.Monday);
        for (var i = 1; i <= 4; i++)
            builder.WithPlayer(new PlayerBuilder().WithId(i).Build());
        var season = builder.Build();

        // Plan one of the three compositions already (1,2 vs 3,4).
        var existing = new PlannedMatchBuilder()
            .WithId(50).WithSeasonId(season.Id).WithDate(season.GetMatchdays()[0])
            .WithPlayers(1, 2, 3, 4).Build();

        SetupSeason(season);
        SetupExisting(season.Id, existing);

        var result = await GenerateScheduleHandler_Handle(season.Id);

        // 4 matchdays, 3 in pool, 1 already planned -> 2 remaining candidates fill 2 open days.
        result.Should().NotBeNull();
        _persisted.Should().HaveCount(2);
        result!.PlannedCount.Should().Be(3);
        result.OpenCount.Should().Be(1);

        // The already-planned composition must not be reassigned.
        var existingKey = Composition(existing);
        _persisted.Select(Composition).Should().NotContain(existingKey);
        _persisted.Select(Composition).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task EmptyPlan_WhenFewerThanFourPlayers()
    {
        var builder = new SeasonBuilder()
            .WithId(1)
            .WithStartDate(new DateOnly(2025, 1, 6))
            .WithEndDate(new DateOnly(2025, 1, 27))
            .WithWeekday(DayOfWeek.Monday);
        for (var i = 1; i <= 3; i++)
            builder.WithPlayer(new PlayerBuilder().WithId(i).Build());
        var season = builder.Build();
        SetupSeason(season);
        SetupExisting(season.Id);

        var result = await GenerateScheduleHandler_Handle(season.Id);

        result.Should().NotBeNull();
        result!.PlannedCount.Should().Be(0);
        result.OpenCount.Should().Be(season.GetMatchdays().Count);
        result.PlannedMatches.Should().BeEmpty();
        _plannedRepo.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<PlannedMatch>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ReturnsNull_ForUnknownSeason()
    {
        _seasonRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Season?)null);

        var result = await GenerateScheduleCommandHandler.Handle(
            new GenerateScheduleCommand(999), _seasonRepo.Object, _generator, _plannedRepo.Object);

        result.Should().BeNull();
    }

    private Task<Winterplein.Application.IO.DTOs.GenerateScheduleResponse?> GenerateScheduleHandler_Handle(int seasonId)
        => GenerateScheduleCommandHandler.Handle(
            new GenerateScheduleCommand(seasonId), _seasonRepo.Object, _generator, _plannedRepo.Object);
}
