using Microsoft.EntityFrameworkCore;
using Winterplein.Domain.Enums;
using Winterplein.Domain.Entities;
using Winterplein.Domain.ValueObjects;
using Winterplein.Infrastructure;
using Winterplein.Infrastructure.Repositories;
using Winterplein.Common.UnitTests.Builders;

namespace Winterplein.Infrastructure.UnitTests;

public class EfPlayerRepositoryTests
{
    private static WinterpleinDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<WinterpleinDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    [Fact]
    public async Task Add_PersistsPlayer_AndAssignsId()
    {
        using var db = CreateContext();
        var repo = new EfPlayerRepository(db);
        var player = new Player(0, new Name("John", "Doe"), Gender.Male);

        var result = await repo.AddAsync(player);

        result.Id.Should().BeGreaterThan(0);
        result.Name.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task GetById_ReturnsPlayer_WhenExists()
    {
        using var db = CreateContext();
        var repo = new EfPlayerRepository(db);
        var added = await repo.AddAsync(new Player(0, new Name("Jane", "Doe"), Gender.Female));

        var result = await repo.GetByIdAsync(added.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(added.Id);
        result.Name.FirstName.Should().Be("Jane");
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenNotFound()
    {
        using var db = CreateContext();
        var repo = new EfPlayerRepository(db);

        var result = await repo.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Remove_DeletesPlayer()
    {
        using var db = CreateContext();
        var repo = new EfPlayerRepository(db);
        var added = await repo.AddAsync(new Player(0, new Name("Test", "User"), Gender.Male));

        await repo.RemoveAsync(added.Id);

        var result = await repo.GetByIdAsync(added.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task Count_ReflectsStoredPlayers()
    {
        using var db = CreateContext();
        var repo = new EfPlayerRepository(db);
        await repo.AddAsync(new Player(0, new Name("A", "B"), Gender.Male));
        await repo.AddAsync(new Player(0, new Name("C", "D"), Gender.Female));

        var count = await repo.CountAsync();

        count.Should().Be(2);
    }
}
