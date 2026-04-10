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
    public void CreateSeasonCommandHandler_ReturnsNewSeason()
    {
        var season = new SeasonBuilder().WithId(5).Build();
        _repo.Setup(r => r.Add(It.IsAny<Season>())).Returns(season);

        var result = CreateSeasonCommandHandler.Handle(new CreateSeasonCommand(
            "Test", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)),
            _repo.Object);

        result.Id.Should().Be(5);
    }

    // --- GetSeasonsQueryHandler ---

    [Fact]
    public void GetSeasonsQueryHandler_ReturnsAllSeasons()
    {
        var seasons = new List<Season> { new SeasonBuilder().Build(), new SeasonBuilder().WithId(2).Build() };
        _repo.Setup(r => r.GetAll()).Returns(seasons);

        var result = GetSeasonsQueryHandler.Handle(new GetSeasonsQuery(), _repo.Object);

        result.Should().HaveCount(2);
    }

    // --- GetSeasonByIdQueryHandler ---

    [Fact]
    public void GetSeasonByIdQueryHandler_ReturnsCorrectSeason()
    {
        var season = new SeasonBuilder().WithId(3).Build();
        _repo.Setup(r => r.GetById(3)).Returns(season);

        var result = GetSeasonByIdQueryHandler.Handle(new GetSeasonByIdQuery(3), _repo.Object);

        result.Should().NotBeNull();
        result!.Id.Should().Be(3);
    }

    [Fact]
    public void GetSeasonByIdQueryHandler_ReturnsNull_ForUnknownId()
    {
        _repo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Season?)null);

        var result = GetSeasonByIdQueryHandler.Handle(new GetSeasonByIdQuery(999), _repo.Object);

        result.Should().BeNull();
    }

    // --- UpdateSeasonCommandHandler ---

    [Fact]
    public void UpdateSeasonCommandHandler_ReturnsUpdatedSeason_WhenSeasonFound()
    {
        var existing = new SeasonBuilder().WithId(1).Build();
        _repo.Setup(r => r.GetById(1)).Returns(existing);
        _repo.Setup(r => r.Update(It.IsAny<Season>())).Returns(true);

        var result = UpdateSeasonCommandHandler.Handle(new UpdateSeasonCommand(
            1, "Updated", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)),
            _repo.Object);

        result.Name.Should().Be("Updated");
    }

    [Fact]
    public void UpdateSeasonCommandHandler_ThrowsKeyNotFoundException_ForUnknownId()
    {
        _repo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Season?)null);

        var act = () => UpdateSeasonCommandHandler.Handle(new UpdateSeasonCommand(
            999, "X", new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)),
            _repo.Object);

        act.Should().Throw<KeyNotFoundException>();
    }

    // --- DeleteSeasonCommandHandler ---

    [Fact]
    public void DeleteSeasonCommandHandler_Succeeds_WhenSeasonDeleted()
    {
        _repo.Setup(r => r.Delete(1)).Returns(true);

        var act = () => DeleteSeasonCommandHandler.Handle(new DeleteSeasonCommand(1), _repo.Object);

        act.Should().NotThrow();
    }

    [Fact]
    public void DeleteSeasonCommandHandler_ThrowsKeyNotFoundException_ForUnknownId()
    {
        _repo.Setup(r => r.Delete(999)).Returns(false);

        var act = () => DeleteSeasonCommandHandler.Handle(new DeleteSeasonCommand(999), _repo.Object);

        act.Should().Throw<KeyNotFoundException>();
    }

    // --- AddSeasonPlayerCommandHandler ---

    [Fact]
    public void AddSeasonPlayerCommandHandler_ReturnsSeasonDto_WhenBothFound()
    {
        var season = new SeasonBuilder().WithId(1).Build();
        var player = new PlayerBuilder().WithId(10).Build();
        _repo.Setup(r => r.GetById(1)).Returns(season);
        _repo.Setup(r => r.Update(It.IsAny<Season>())).Returns(true);
        _playerRepo.Setup(r => r.GetById(10)).Returns(player);

        var result = AddSeasonPlayerCommandHandler.Handle(new AddSeasonPlayerCommand(1, 10), _repo.Object, _playerRepo.Object);

        result.Should().NotBeNull();
        result!.Players.Should().Contain(p => p.Id == 10);
    }

    [Fact]
    public void AddSeasonPlayerCommandHandler_ReturnsNull_WhenSeasonNotFound()
    {
        _repo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Season?)null);

        var result = AddSeasonPlayerCommandHandler.Handle(new AddSeasonPlayerCommand(999, 1), _repo.Object, _playerRepo.Object);

        result.Should().BeNull();
    }

    [Fact]
    public void AddSeasonPlayerCommandHandler_ReturnsNull_WhenPlayerNotFound()
    {
        var season = new SeasonBuilder().WithId(1).Build();
        _repo.Setup(r => r.GetById(1)).Returns(season);
        _playerRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Player?)null);

        var result = AddSeasonPlayerCommandHandler.Handle(new AddSeasonPlayerCommand(1, 999), _repo.Object, _playerRepo.Object);

        result.Should().BeNull();
    }

    // --- RemoveSeasonPlayerCommandHandler ---

    [Fact]
    public void RemoveSeasonPlayerCommandHandler_Succeeds_WhenPlayerRemoved()
    {
        var players = Enumerable.Range(1, 5).Select(i => new PlayerBuilder().WithId(i).Build()).ToList();
        var season = new SeasonBuilder().WithId(1).Build();
        players.ForEach(season.AddPlayer);
        _repo.Setup(r => r.GetById(1)).Returns(season);
        _repo.Setup(r => r.Update(It.IsAny<Season>())).Returns(true);

        var act = () => RemoveSeasonPlayerCommandHandler.Handle(new RemoveSeasonPlayerCommand(1, 1), _repo.Object);

        act.Should().NotThrow();
    }

    [Fact]
    public void RemoveSeasonPlayerCommandHandler_ThrowsKeyNotFoundException_WhenSeasonNotFound()
    {
        _repo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Season?)null);

        var act = () => RemoveSeasonPlayerCommandHandler.Handle(new RemoveSeasonPlayerCommand(999, 1), _repo.Object);

        act.Should().Throw<KeyNotFoundException>();
    }

    // --- GetSeasonPlayersQueryHandler ---

    [Fact]
    public void GetSeasonPlayersQueryHandler_ReturnsPlayerDtos_WhenSeasonFound()
    {
        var player = new PlayerBuilder().WithId(1).Build();
        var season = new SeasonBuilder().WithPlayer(player).Build();
        _repo.Setup(r => r.GetById(1)).Returns(season);

        var result = GetSeasonPlayersQueryHandler.Handle(new GetSeasonPlayersQuery(1), _repo.Object);

        result.Should().NotBeNull();
        result!.Should().HaveCount(1);
        result[0].Id.Should().Be(1);
    }

    [Fact]
    public void GetSeasonPlayersQueryHandler_ReturnsNull_WhenSeasonNotFound()
    {
        _repo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Season?)null);

        var result = GetSeasonPlayersQueryHandler.Handle(new GetSeasonPlayersQuery(999), _repo.Object);

        result.Should().BeNull();
    }
}
