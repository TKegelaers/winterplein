using Moq;
using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;
using Winterplein.Application.IO.Queries;
using Winterplein.Application.CommandHandlers.CreateSeason;
using Winterplein.Application.CommandHandlers.UpdateSeason;
using Winterplein.Application.CommandHandlers.DeleteSeason;
using Winterplein.Application.CommandHandlers.AddSeasonPlayer;
using Winterplein.Application.CommandHandlers.RemoveSeasonPlayer;
using Winterplein.Application.QueryHandlers.GetSeasonById;
using Winterplein.Application.QueryHandlers.GetSeasons;
using Winterplein.Application.QueryHandlers.GetSeasonPlayers;
using Winterplein.Domain.Entities;
using Winterplein.Common.UnitTests.Builders;

namespace Winterplein.Application.UnitTests.Seasons;

public class SeasonHandlerTests
{
    private readonly Mock<ISeasonRepository> _repo = new();
    private readonly Mock<IPlayerRepository> _playerRepo = new();

    // --- CreateSeasonCommandHandler ---

    [Fact]
    public async Task CreateSeasonCommandHandler_ReturnsNewSeason()
    {
        var season = new SeasonBuilder().WithId(5).Build();
        _repo.Setup(r => r.AddAsync(It.IsAny<Season>(), It.IsAny<CancellationToken>())).ReturnsAsync(season);

        var result = await CreateSeasonCommandHandler.Handle(new CreateSeasonCommand(
            "Test", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)),
            _repo.Object);

        result.Id.Should().Be(5);
    }

    // --- GetSeasonsQueryHandler ---

    [Fact]
    public async Task GetSeasonsQueryHandler_ReturnsAllSeasons()
    {
        var seasons = new List<Season> { new SeasonBuilder().Build(), new SeasonBuilder().WithId(2).Build() };
        _repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(seasons);

        var result = await GetSeasonsQueryHandler.Handle(new GetSeasonsQuery(), _repo.Object);

        result.Should().HaveCount(2);
    }

    // --- GetSeasonByIdQueryHandler ---

    [Fact]
    public async Task GetSeasonByIdQueryHandler_ReturnsCorrectSeason()
    {
        var season = new SeasonBuilder().WithId(3).Build();
        _repo.Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(season);

        var result = await GetSeasonByIdQueryHandler.Handle(new GetSeasonByIdQuery(3), _repo.Object);

        result.Should().NotBeNull();
        result!.Id.Should().Be(3);
    }

    [Fact]
    public async Task GetSeasonByIdQueryHandler_ReturnsNull_ForUnknownId()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Season?)null);

        var result = await GetSeasonByIdQueryHandler.Handle(new GetSeasonByIdQuery(999), _repo.Object);

        result.Should().BeNull();
    }

    // --- UpdateSeasonCommandHandler ---

    [Fact]
    public async Task UpdateSeasonCommandHandler_ReturnsUpdatedSeason_WhenSeasonFound()
    {
        var existing = new SeasonBuilder().WithId(1).Build();
        var updated = new SeasonBuilder().WithId(1).WithName("Updated").Build();
        _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Season>(), It.IsAny<CancellationToken>())).ReturnsAsync(updated);

        var result = await UpdateSeasonCommandHandler.Handle(new UpdateSeasonCommand(
            1, "Updated", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)),
            _repo.Object);

        result.Name.Should().Be("Updated");
    }

    [Fact]
    public async Task UpdateSeasonCommandHandler_ThrowsKeyNotFoundException_ForUnknownId()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Season?)null);

        var act = () => UpdateSeasonCommandHandler.Handle(new UpdateSeasonCommand(
            999, "X", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)),
            _repo.Object);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    // --- DeleteSeasonCommandHandler ---

    [Fact]
    public async Task DeleteSeasonCommandHandler_DoesNotThrow_WhenSeasonExists()
    {
        _repo.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var act = () => DeleteSeasonCommandHandler.Handle(new DeleteSeasonCommand(1), _repo.Object);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task DeleteSeasonCommandHandler_ThrowsKeyNotFoundException_ForUnknownId()
    {
        _repo.Setup(r => r.DeleteAsync(999, It.IsAny<CancellationToken>())).ThrowsAsync(new KeyNotFoundException());

        var act = () => DeleteSeasonCommandHandler.Handle(new DeleteSeasonCommand(999), _repo.Object);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    // --- AddSeasonPlayerCommandHandler ---

    [Fact]
    public async Task AddSeasonPlayerCommandHandler_ReturnsSeason_WhenBothFound()
    {
        var season = new SeasonBuilder().WithId(1).Build();
        var player = new PlayerBuilder().WithId(10).Build();
        _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(season);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Season>(), It.IsAny<CancellationToken>())).ReturnsAsync(season);
        _playerRepo.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(player);

        var result = await AddSeasonPlayerCommandHandler.Handle(new AddSeasonPlayerCommand(1, 10), _repo.Object, _playerRepo.Object);

        result.Should().NotBeNull();
        result.Players.Should().Contain(p => p.Id == 10);
    }

    [Fact]
    public async Task AddSeasonPlayerCommandHandler_ThrowsKeyNotFoundException_WhenSeasonNotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Season?)null);

        var act = () => AddSeasonPlayerCommandHandler.Handle(new AddSeasonPlayerCommand(999, 1), _repo.Object, _playerRepo.Object);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task AddSeasonPlayerCommandHandler_ThrowsKeyNotFoundException_WhenPlayerNotFound()
    {
        var season = new SeasonBuilder().WithId(1).Build();
        _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(season);
        _playerRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Player?)null);

        var act = () => AddSeasonPlayerCommandHandler.Handle(new AddSeasonPlayerCommand(1, 999), _repo.Object, _playerRepo.Object);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    // --- RemoveSeasonPlayerCommandHandler ---

    [Fact]
    public async Task RemoveSeasonPlayerCommandHandler_Succeeds_WhenPlayerRemoved()
    {
        var players = Enumerable.Range(1, 5).Select(i => new PlayerBuilder().WithId(i).Build()).ToList();
        var season = new SeasonBuilder().WithId(1).Build();
        players.ForEach(season.AddPlayer);
        _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(season);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Season>(), It.IsAny<CancellationToken>())).ReturnsAsync(season);

        var act = () => RemoveSeasonPlayerCommandHandler.Handle(new RemoveSeasonPlayerCommand(1, 1), _repo.Object);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task RemoveSeasonPlayerCommandHandler_ThrowsKeyNotFoundException_WhenSeasonNotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Season?)null);

        var act = () => RemoveSeasonPlayerCommandHandler.Handle(new RemoveSeasonPlayerCommand(999, 1), _repo.Object);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    // --- GetSeasonPlayersQueryHandler ---

    [Fact]
    public async Task GetSeasonPlayersQueryHandler_ReturnsPlayers_WhenSeasonFound()
    {
        var player = new PlayerBuilder().WithId(1).Build();
        var season = new SeasonBuilder().WithPlayer(player).Build();
        _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(season);

        var result = await GetSeasonPlayersQueryHandler.Handle(new GetSeasonPlayersQuery(1), _repo.Object);

        result.Should().NotBeNull();
        result!.Should().HaveCount(1);
        result[0].Id.Should().Be(1);
    }

    [Fact]
    public async Task GetSeasonPlayersQueryHandler_ReturnsNull_WhenSeasonNotFound()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Season?)null);

        var result = await GetSeasonPlayersQueryHandler.Handle(new GetSeasonPlayersQuery(999), _repo.Object);

        result.Should().BeNull();
    }
}
