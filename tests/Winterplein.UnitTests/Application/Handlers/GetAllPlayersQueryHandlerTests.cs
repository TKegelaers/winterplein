using Moq;
using Winterplein.Application.Interfaces;
using Winterplein.Application.Queries.GetAllPlayers;
using Winterplein.UnitTests.Common.Builders;

namespace Winterplein.UnitTests.Application.Handlers;

public class GetAllPlayersQueryHandlerTests
{
    private readonly Mock<IPlayerRepository> _repo = new();
    private readonly GetAllPlayersQueryHandler _sut;

    public GetAllPlayersQueryHandlerTests() => _sut = new GetAllPlayersQueryHandler(_repo.Object);

    [Fact]
    public async Task Handle_ReturnsAllPlayersAsDtos()
    {
        var players = new[]
        {
            new PlayerBuilder().WithId(1).Build(),
            new PlayerBuilder().WithId(2).Build()
        };
        _repo.Setup(r => r.GetAll()).Returns(players);

        var result = await _sut.Handle(new GetAllPlayersQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Id.Should().Be(1);
        result[1].Id.Should().Be(2);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoPlayers()
    {
        _repo.Setup(r => r.GetAll()).Returns(Array.Empty<Winterplein.Domain.Entities.Player>());

        var result = await _sut.Handle(new GetAllPlayersQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }
}
