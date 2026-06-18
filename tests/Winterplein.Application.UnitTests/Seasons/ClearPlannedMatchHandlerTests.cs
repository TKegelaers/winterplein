using Moq;
using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;
using Winterplein.Application.CommandHandlers.ClearPlannedMatch;
using Winterplein.Domain.Entities;
using Winterplein.Common.UnitTests.Builders;

namespace Winterplein.Application.UnitTests.Seasons;

public class ClearPlannedMatchHandlerTests
{
    private readonly Mock<ISeasonRepository> _seasonRepo = new();
    private readonly Mock<IPlannedMatchRepository> _plannedRepo = new();

    private static Season SeasonWithId(int id = 1)
        => new SeasonBuilder()
            .WithId(id)
            .WithStartDate(new DateOnly(2025, 1, 6))
            .WithEndDate(new DateOnly(2025, 1, 27))
            .WithWeekday(DayOfWeek.Monday)
            .Build();

    private Task Handle(int seasonId, DateOnly date)
        => ClearPlannedMatchCommandHandler.Handle(
            new ClearPlannedMatchCommand(seasonId, date), _seasonRepo.Object, _plannedRepo.Object);

    [Fact]
    public async Task Deletes_PlannedMatch_AtDate()
    {
        var season = SeasonWithId();
        var date = new DateOnly(2025, 1, 13);
        _seasonRepo.Setup(r => r.GetByIdAsync(season.Id, It.IsAny<CancellationToken>())).ReturnsAsync(season);
        _plannedRepo.Setup(r => r.DeleteBySeasonAndDateAsync(season.Id, date, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Handle(season.Id, date);

        _plannedRepo.Verify(r => r.DeleteBySeasonAndDateAsync(season.Id, date, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Throws_KeyNotFound_WhenSeasonUnknown()
    {
        _seasonRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Season?)null);

        var act = () => Handle(999, new DateOnly(2025, 1, 13));

        await act.Should().ThrowAsync<KeyNotFoundException>();
        _plannedRepo.Verify(r => r.DeleteBySeasonAndDateAsync(It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Throws_KeyNotFound_WhenNoPlannedMatchAtDate()
    {
        var season = SeasonWithId();
        var date = new DateOnly(2025, 1, 13);
        _seasonRepo.Setup(r => r.GetByIdAsync(season.Id, It.IsAny<CancellationToken>())).ReturnsAsync(season);
        _plannedRepo.Setup(r => r.DeleteBySeasonAndDateAsync(season.Id, date, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var act = () => Handle(season.Id, date);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
