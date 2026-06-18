using Moq;
using Winterplein.Application.Ports;
using Winterplein.Application.IO.Queries;
using Winterplein.Application.QueryHandlers.GetSeasonSchedule;
using Winterplein.Domain.Entities;
using Winterplein.Common.UnitTests.Builders;

namespace Winterplein.Application.UnitTests.Seasons;

public class GetSeasonScheduleHandlerTests
{
    private readonly Mock<ISeasonRepository> _seasonRepo = new();
    private readonly Mock<IPlannedMatchRepository> _plannedRepo = new();

    private void SetupSeason(Season season)
        => _seasonRepo.Setup(r => r.GetByIdAsync(season.Id, It.IsAny<CancellationToken>())).ReturnsAsync(season);

    private void SetupPlanned(int seasonId, params PlannedMatch[] planned)
        => _plannedRepo.Setup(r => r.GetAllBySeasonAsync(seasonId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(planned.ToList());

    // A season with 4 Mondays: 6, 13, 20, 27 (Jan 2025).
    private static Season FourMatchdaySeason(int id = 1)
        => new SeasonBuilder()
            .WithId(id)
            .WithStartDate(new DateOnly(2025, 1, 6))
            .WithEndDate(new DateOnly(2025, 1, 27))
            .WithWeekday(DayOfWeek.Monday)
            .Build();

    private Task<Winterplein.Application.IO.DTOs.SeasonScheduleResponse?> Handle(int seasonId)
        => GetSeasonScheduleQueryHandler.Handle(
            new GetSeasonScheduleQuery(seasonId), _seasonRepo.Object, _plannedRepo.Object);

    [Fact]
    public async Task BuildsEntry_PerMatchday_InDateOrder()
    {
        var season = FourMatchdaySeason();
        var matchdays = season.GetMatchdays();
        SetupSeason(season);
        SetupPlanned(season.Id);

        var result = await Handle(season.Id);

        result.Should().NotBeNull();
        result!.Entries.Should().HaveCount(matchdays.Count);
        result.Entries.Select(e => e.Date).Should().BeInAscendingOrder();
        result.Entries.Select(e => e.Date).Should().Equal(matchdays.OrderBy(d => d));
    }

    [Fact]
    public async Task MarksMatchday_Planned_WhenPlannedMatchExists()
    {
        var season = FourMatchdaySeason();
        var matchdays = season.GetMatchdays();
        var planned = new PlannedMatchBuilder()
            .WithId(7).WithSeasonId(season.Id).WithDate(matchdays[1]).WithPlayers(1, 2, 3, 4).Build();
        SetupSeason(season);
        SetupPlanned(season.Id, planned);

        var result = await Handle(season.Id);

        var entry = result!.Entries.Single(e => e.Date == matchdays[1]);
        entry.IsPlanned.Should().BeTrue();
        entry.PlannedMatch.Should().NotBeNull();
        entry.PlannedMatch!.Id.Should().Be(7);
        entry.PlannedMatch.Date.Should().Be(matchdays[1]);
    }

    [Fact]
    public async Task MarksMatchday_Open_WhenNoPlannedMatch()
    {
        var season = FourMatchdaySeason();
        var matchdays = season.GetMatchdays();
        var planned = new PlannedMatchBuilder()
            .WithId(7).WithSeasonId(season.Id).WithDate(matchdays[1]).WithPlayers(1, 2, 3, 4).Build();
        SetupSeason(season);
        SetupPlanned(season.Id, planned);

        var result = await Handle(season.Id);

        var openEntry = result!.Entries.Single(e => e.Date == matchdays[0]);
        openEntry.IsPlanned.Should().BeFalse();
        openEntry.PlannedMatch.Should().BeNull();
    }

    [Fact]
    public async Task ReturnsNull_ForUnknownSeason()
    {
        _seasonRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Season?)null);

        var result = await Handle(999);

        result.Should().BeNull();
        _plannedRepo.Verify(r => r.GetAllBySeasonAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
