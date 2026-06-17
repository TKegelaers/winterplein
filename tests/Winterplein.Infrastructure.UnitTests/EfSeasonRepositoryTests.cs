using Microsoft.EntityFrameworkCore;
using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;
using Winterplein.Infrastructure;
using Winterplein.Infrastructure.Repositories;
using Winterplein.Common.UnitTests.Builders;

namespace Winterplein.Infrastructure.UnitTests;

public class EfSeasonRepositoryTests
{
    private static WinterpleinDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<WinterpleinDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static Season BuildSeason(int id = 0) =>
        new(id, "Summer 2025", new DateOnly(2025, 1, 6), new DateOnly(2025, 3, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0));

    private static Player BuildPlayer(string first = "Jane", string last = "Doe") =>
        new(0, new Name(first, last), Gender.Female);

    [Fact]
    public async Task Add_PersistsSeason_WithPlayers()
    {
        using var db = CreateContext();
        var playerRepo = new EfPlayerRepository(db);
        var seasonRepo = new EfSeasonRepository(db);

        var player = await playerRepo.AddAsync(BuildPlayer());
        var season = BuildSeason();
        season.AddPlayer(player);

        var result = await seasonRepo.AddAsync(season);

        result.Id.Should().BeGreaterThan(0);
        result.Players.Should().HaveCount(1);
        result.Players[0].Id.Should().Be(player.Id);
    }

    [Fact]
    public async Task GetById_ReturnsSeason_WithPlayers()
    {
        using var db = CreateContext();
        var playerRepo = new EfPlayerRepository(db);
        var seasonRepo = new EfSeasonRepository(db);

        var player = await playerRepo.AddAsync(BuildPlayer());
        var season = BuildSeason();
        season.AddPlayer(player);
        var added = await seasonRepo.AddAsync(season);

        var result = await seasonRepo.GetByIdAsync(added.Id);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Summer 2025");
        result.Players.Should().HaveCount(1);
    }

    [Fact]
    public async Task Update_UpdatesScalarsAndPlayers()
    {
        using var db = CreateContext();
        var playerRepo = new EfPlayerRepository(db);
        var seasonRepo = new EfSeasonRepository(db);

        var player1 = await playerRepo.AddAsync(BuildPlayer("Alice", "A"));
        var player2 = await playerRepo.AddAsync(BuildPlayer("Bob", "B"));
        var season = BuildSeason();
        season.AddPlayer(player1);
        var added = await seasonRepo.AddAsync(season);

        // Update: change name and swap player1 for player2
        var updated = new Season(added.Id, "Winter 2025", added.StartDate, added.EndDate,
            added.Weekday, added.StartHour, added.EndHour, [player2]);

        var result = await seasonRepo.UpdateAsync(updated);

        result.Name.Should().Be("Winter 2025");
        result.Players.Should().HaveCount(1);
        result.Players[0].Id.Should().Be(player2.Id);
    }

    [Fact]
    public async Task Delete_RemovesSeason()
    {
        using var db = CreateContext();
        var seasonRepo = new EfSeasonRepository(db);
        var added = await seasonRepo.AddAsync(BuildSeason());

        await seasonRepo.DeleteAsync(added.Id);

        var result = await seasonRepo.GetByIdAsync(added.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_ReturnsAllSeasons()
    {
        using var db = CreateContext();
        var seasonRepo = new EfSeasonRepository(db);
        await seasonRepo.AddAsync(BuildSeason());
        await seasonRepo.AddAsync(new Season(0, "Winter 2025", new DateOnly(2025, 10, 1),
            new DateOnly(2025, 12, 31), DayOfWeek.Wednesday, new TimeOnly(19, 0), new TimeOnly(21, 0)));

        var result = await seasonRepo.GetAllAsync();

        result.Should().HaveCount(2);
    }
}
