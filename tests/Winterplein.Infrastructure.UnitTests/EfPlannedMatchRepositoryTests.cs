using Microsoft.EntityFrameworkCore;
using Winterplein.Common.UnitTests.Builders;
using Winterplein.Infrastructure;
using Winterplein.Infrastructure.Repositories;

namespace Winterplein.Infrastructure.UnitTests;

public class EfPlannedMatchRepositoryTests
{
    private static WinterpleinDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<WinterpleinDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    [Fact]
    public async Task DeleteBySeasonAndDate_RemovesMatch_AndReturnsTrue()
    {
        using var db = CreateContext();
        var repo = new EfPlannedMatchRepository(db);
        var date = new DateOnly(2025, 1, 6);
        await repo.AddRangeAsync([
            new PlannedMatchBuilder().WithId(1).WithSeasonId(1).WithDate(date).Build()
        ]);

        var result = await repo.DeleteBySeasonAndDateAsync(1, date);

        result.Should().BeTrue();
        var remaining = await repo.GetAllBySeasonAsync(1);
        remaining.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteBySeasonAndDate_ReturnsFalse_WhenNoMatchAtDate()
    {
        using var db = CreateContext();
        var repo = new EfPlannedMatchRepository(db);
        await repo.AddRangeAsync([
            new PlannedMatchBuilder().WithId(1).WithSeasonId(1).WithDate(new DateOnly(2025, 1, 6)).Build()
        ]);

        var result = await repo.DeleteBySeasonAndDateAsync(1, new DateOnly(2025, 1, 13));

        result.Should().BeFalse();
        var remaining = await repo.GetAllBySeasonAsync(1);
        remaining.Should().HaveCount(1);
    }

    [Fact]
    public async Task DeleteBySeasonAndDate_DoesNotAffectOtherSeasonsOrDates()
    {
        using var db = CreateContext();
        var repo = new EfPlannedMatchRepository(db);
        var targetDate = new DateOnly(2025, 1, 6);
        var otherDate = new DateOnly(2025, 1, 13);
        await repo.AddRangeAsync([
            new PlannedMatchBuilder().WithId(1).WithSeasonId(1).WithDate(targetDate).Build(),
            new PlannedMatchBuilder().WithId(2).WithSeasonId(1).WithDate(otherDate).Build(),
            new PlannedMatchBuilder().WithId(3).WithSeasonId(2).WithDate(targetDate).Build()
        ]);

        var result = await repo.DeleteBySeasonAndDateAsync(1, targetDate);

        result.Should().BeTrue();
        var season1 = await repo.GetAllBySeasonAsync(1);
        season1.Should().ContainSingle(pm => pm.Date == otherDate);
        var season2 = await repo.GetAllBySeasonAsync(2);
        season2.Should().ContainSingle(pm => pm.Date == targetDate);
    }

    [Fact]
    public async Task DeleteAllBySeason_RemovesEveryMatchForSeason()
    {
        using var db = CreateContext();
        var repo = new EfPlannedMatchRepository(db);
        await repo.AddRangeAsync([
            new PlannedMatchBuilder().WithId(1).WithSeasonId(1).WithDate(new DateOnly(2025, 1, 6)).Build(),
            new PlannedMatchBuilder().WithId(2).WithSeasonId(1).WithDate(new DateOnly(2025, 1, 13)).Build()
        ]);

        await repo.DeleteAllBySeasonAsync(1);

        var remaining = await repo.GetAllBySeasonAsync(1);
        remaining.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteAllBySeason_IsNoOp_WhenSeasonHasNoMatches()
    {
        using var db = CreateContext();
        var repo = new EfPlannedMatchRepository(db);

        var act = async () => await repo.DeleteAllBySeasonAsync(99);

        await act.Should().NotThrowAsync();
        var remaining = await repo.GetAllBySeasonAsync(99);
        remaining.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteAllBySeason_DoesNotAffectOtherSeasons()
    {
        using var db = CreateContext();
        var repo = new EfPlannedMatchRepository(db);
        await repo.AddRangeAsync([
            new PlannedMatchBuilder().WithId(1).WithSeasonId(1).WithDate(new DateOnly(2025, 1, 6)).Build(),
            new PlannedMatchBuilder().WithId(2).WithSeasonId(2).WithDate(new DateOnly(2025, 1, 6)).Build()
        ]);

        await repo.DeleteAllBySeasonAsync(1);

        var season1 = await repo.GetAllBySeasonAsync(1);
        season1.Should().BeEmpty();
        var season2 = await repo.GetAllBySeasonAsync(2);
        season2.Should().HaveCount(1);
    }
}
