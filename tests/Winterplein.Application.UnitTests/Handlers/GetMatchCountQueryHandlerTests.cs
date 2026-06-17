using Moq;
using Winterplein.Application.Ports;
using Winterplein.Application.IO.Queries;
using Winterplein.Application.QueryHandlers.GetMatchCount;

namespace Winterplein.Application.UnitTests.Handlers;

public class GetMatchCountQueryHandlerTests
{
    private readonly Mock<IPlayerRepository> _repo = new();
    private readonly Mock<IMatchGeneratorService> _generator = new();

    [Fact]
    public async Task Handle_ReturnsCalculatedMatchCount()
    {
        _repo.Setup(r => r.CountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(10);
        _generator.Setup(g => g.CalculateMatchCount(10)).Returns(630);

        var result = await GetMatchCountQueryHandler.Handle(new GetMatchCountQuery(), _repo.Object, _generator.Object);

        result.Count.Should().Be(630);
    }
}
