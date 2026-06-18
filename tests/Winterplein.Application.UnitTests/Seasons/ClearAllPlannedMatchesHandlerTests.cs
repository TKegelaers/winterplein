using Moq;
using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;
using Winterplein.Application.CommandHandlers.ClearAllPlannedMatches;
using Winterplein.Domain.Entities;
using Winterplein.Common.UnitTests.Builders;

namespace Winterplein.Application.UnitTests.Seasons;

public class ClearAllPlannedMatchesHandlerTests
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

    private Task Handle(int seasonId)
        => ClearAllPlannedMatchesCommandHandler.Handle(
            new ClearAllPlannedMatchesCommand(seasonId), _seasonRepo.Object, _plannedRepo.Object);

    [Fact]
    public async Task Deletes_AllPlannedMatches_ForSeason()
    {
        var season = SeasonWithId();
        _seasonRepo.Setup(r => r.GetByIdAsync(season.Id, It.IsAny<CancellationToken>())).ReturnsAsync(season);

        await Handle(season.Id);

        _plannedRepo.Verify(r => r.DeleteAllBySeasonAsync(season.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DoesNotThrow_WhenSeasonAlreadyEmpty()
    {
        var season = SeasonWithId();
        _seasonRepo.Setup(r => r.GetByIdAsync(season.Id, It.IsAny<CancellationToken>())).ReturnsAsync(season);
        _plannedRepo.Setup(r => r.DeleteAllBySeasonAsync(season.Id, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var act = () => Handle(season.Id);

        await act.Should().NotThrowAsync();
        _plannedRepo.Verify(r => r.DeleteAllBySeasonAsync(season.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Throws_KeyNotFound_WhenSeasonUnknown()
    {
        _seasonRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Season?)null);

        var act = () => Handle(999);

        await act.Should().ThrowAsync<KeyNotFoundException>();
        _plannedRepo.Verify(r => r.DeleteAllBySeasonAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
