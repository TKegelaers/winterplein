using Moq;
using Winterplein.Application.Ports;
using Winterplein.Application.IO.Queries;
using Winterplein.Application.QueryHandlers.GetAllPlayers;
using Winterplein.Common.UnitTests.Builders;

namespace Winterplein.Application.UnitTests.Handlers;

public class GetAllPlayersQueryHandlerTests
{
    private readonly Mock<IPlayerRepository> _repo = new();

    [Fact]
    public async Task Handle_ReturnsAllPlayersAsDtos()
    {
        var players = new[]
        {
            new PlayerBuilder().WithId(1).Build(),
            new PlayerBuilder().WithId(2).Build()
        };
        _repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(players);

        var result = await GetAllPlayersQueryHandler.Handle(new GetAllPlayersQuery(), _repo.Object);

        result.Should().HaveCount(2);
        result[0].Id.Should().Be(1);
        result[1].Id.Should().Be(2);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoPlayers()
    {
        _repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Array.Empty<Winterplein.Domain.Entities.Player>());

        var result = await GetAllPlayersQueryHandler.Handle(new GetAllPlayersQuery(), _repo.Object);

        result.Should().BeEmpty();
    }
}
