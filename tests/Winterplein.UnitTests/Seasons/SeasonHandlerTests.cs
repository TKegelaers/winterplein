using Moq;
using Winterplein.Application.Interfaces;
using Winterplein.Application.Seasons;
using Winterplein.Domain.Entities;
using Winterplein.UnitTests.Common.Builders;

namespace Winterplein.UnitTests.Seasons;

public class SeasonHandlerTests
{
    private readonly Mock<ISeasonRepository> _repo = new();
    private readonly Mock<IPlayerRepository> _playerRepo = new();

    // --- CreateSeasonCommandHandler ---

    [Fact]
    public async Task CreateSeasonCommandHandler_ReturnsNewId()
    {
        var season = new SeasonBuilder().WithId(5).Build();
        _repo.Setup(r => r.Add(It.IsAny<Season>())).Returns(season);
        var handler = new CreateSeasonCommandHandler(_repo.Object);

        var result = await handler.Handle(new CreateSeasonCommand(
            "Test", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)),
            CancellationToken.None);

        result.Should().Be(5);
    }

    // --- GetSeasonsQueryHandler ---

    [Fact]
    public async Task GetSeasonsQueryHandler_ReturnsAllSeasons()
    {
        var seasons = new List<Season> { new SeasonBuilder().Build(), new SeasonBuilder().WithId(2).Build() };
        _repo.Setup(r => r.GetAll()).Returns(seasons);
        var handler = new GetSeasonsQueryHandler(_repo.Object);

        var result = await handler.Handle(new GetSeasonsQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }

    // --- GetSeasonByIdQueryHandler ---

    [Fact]
    public async Task GetSeasonByIdQueryHandler_ReturnsCorrectSeason()
    {
        var season = new SeasonBuilder().WithId(3).Build();
        _repo.Setup(r => r.GetById(3)).Returns(season);
        var handler = new GetSeasonByIdQueryHandler(_repo.Object);

        var result = await handler.Handle(new GetSeasonByIdQuery(3), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(3);
    }

    [Fact]
    public async Task GetSeasonByIdQueryHandler_ReturnsNull_ForUnknownId()
    {
        _repo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Season?)null);
        var handler = new GetSeasonByIdQueryHandler(_repo.Object);

        var result = await handler.Handle(new GetSeasonByIdQuery(999), CancellationToken.None);

        result.Should().BeNull();
    }

    // --- UpdateSeasonCommandHandler ---

    [Fact]
    public async Task UpdateSeasonCommandHandler_ReturnsTrue_WhenSeasonFound()
    {
        var existing = new SeasonBuilder().WithId(1).Build();
        _repo.Setup(r => r.GetById(1)).Returns(existing);
        _repo.Setup(r => r.Update(It.IsAny<Season>())).Returns(true);
        var handler = new UpdateSeasonCommandHandler(_repo.Object);

        var result = await handler.Handle(new UpdateSeasonCommand(
            1, "Updated", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)),
            CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateSeasonCommandHandler_ReturnsFalse_ForUnknownId()
    {
        _repo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Season?)null);
        var handler = new UpdateSeasonCommandHandler(_repo.Object);

        var result = await handler.Handle(new UpdateSeasonCommand(
            999, "X", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)),
            CancellationToken.None);

        result.Should().BeFalse();
    }

    // --- DeleteSeasonCommandHandler ---

    [Fact]
    public async Task DeleteSeasonCommandHandler_ReturnsTrue_WhenSeasonDeleted()
    {
        _repo.Setup(r => r.Delete(1)).Returns(true);
        var handler = new DeleteSeasonCommandHandler(_repo.Object);

        var result = await handler.Handle(new DeleteSeasonCommand(1), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteSeasonCommandHandler_ReturnsFalse_ForUnknownId()
    {
        _repo.Setup(r => r.Delete(999)).Returns(false);
        var handler = new DeleteSeasonCommandHandler(_repo.Object);

        var result = await handler.Handle(new DeleteSeasonCommand(999), CancellationToken.None);

        result.Should().BeFalse();
    }

    // --- AddSeasonPlayerCommandHandler ---

    [Fact]
    public async Task AddSeasonPlayerCommandHandler_ReturnsSeasonDto_WhenBothFound()
    {
        var season = new SeasonBuilder().WithId(1).Build();
        var player = new PlayerBuilder().WithId(10).Build();
        _repo.Setup(r => r.GetById(1)).Returns(season);
        _repo.Setup(r => r.Update(It.IsAny<Season>())).Returns(true);
        _playerRepo.Setup(r => r.GetById(10)).Returns(player);
        var handler = new AddSeasonPlayerCommandHandler(_repo.Object, _playerRepo.Object);

        var result = await handler.Handle(new AddSeasonPlayerCommand(1, 10), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Players.Should().Contain(p => p.Id == 10);
    }

    [Fact]
    public async Task AddSeasonPlayerCommandHandler_ReturnsNull_WhenSeasonNotFound()
    {
        _repo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Season?)null);
        var handler = new AddSeasonPlayerCommandHandler(_repo.Object, _playerRepo.Object);

        var result = await handler.Handle(new AddSeasonPlayerCommand(999, 1), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddSeasonPlayerCommandHandler_ReturnsNull_WhenPlayerNotFound()
    {
        var season = new SeasonBuilder().WithId(1).Build();
        _repo.Setup(r => r.GetById(1)).Returns(season);
        _playerRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Player?)null);
        var handler = new AddSeasonPlayerCommandHandler(_repo.Object, _playerRepo.Object);

        var result = await handler.Handle(new AddSeasonPlayerCommand(1, 999), CancellationToken.None);

        result.Should().BeNull();
    }

    // --- RemoveSeasonPlayerCommandHandler ---

    [Fact]
    public async Task RemoveSeasonPlayerCommandHandler_ReturnsTrue_WhenPlayerRemoved()
    {
        var players = Enumerable.Range(1, 5).Select(i => new PlayerBuilder().WithId(i).Build()).ToList();
        var season = new SeasonBuilder().WithId(1).Build();
        players.ForEach(season.AddPlayer);
        _repo.Setup(r => r.GetById(1)).Returns(season);
        _repo.Setup(r => r.Update(It.IsAny<Season>())).Returns(true);
        var handler = new RemoveSeasonPlayerCommandHandler(_repo.Object);

        var result = await handler.Handle(new RemoveSeasonPlayerCommand(1, 1), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task RemoveSeasonPlayerCommandHandler_ReturnsFalse_WhenSeasonNotFound()
    {
        _repo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Season?)null);
        var handler = new RemoveSeasonPlayerCommandHandler(_repo.Object);

        var result = await handler.Handle(new RemoveSeasonPlayerCommand(999, 1), CancellationToken.None);

        result.Should().BeFalse();
    }

    // --- GetSeasonPlayersQueryHandler ---

    [Fact]
    public async Task GetSeasonPlayersQueryHandler_ReturnsPlayerDtos_WhenSeasonFound()
    {
        var player = new PlayerBuilder().WithId(1).Build();
        var season = new SeasonBuilder().WithPlayer(player).Build();
        _repo.Setup(r => r.GetById(1)).Returns(season);
        var handler = new GetSeasonPlayersQueryHandler(_repo.Object);

        var result = await handler.Handle(new GetSeasonPlayersQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Should().HaveCount(1);
        result[0].Id.Should().Be(1);
    }

    [Fact]
    public async Task GetSeasonPlayersQueryHandler_ReturnsNull_WhenSeasonNotFound()
    {
        _repo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Season?)null);
        var handler = new GetSeasonPlayersQueryHandler(_repo.Object);

        var result = await handler.Handle(new GetSeasonPlayersQuery(999), CancellationToken.None);

        result.Should().BeNull();
    }
}
