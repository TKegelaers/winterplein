using Moq;
using Winterplein.Application.Interfaces;
using Winterplein.Application.Queries.GetMatchCount;

namespace Winterplein.UnitTests.Application.Handlers;

public class GetMatchCountQueryHandlerTests
{
    private readonly Mock<IPlayerRepository> _repo = new();
    private readonly Mock<IMatchGeneratorService> _generator = new();
    private readonly GetMatchCountQueryHandler _sut;

    public GetMatchCountQueryHandlerTests() =>
        _sut = new GetMatchCountQueryHandler(_repo.Object, _generator.Object);

    [Fact]
    public async Task Handle_ReturnsCalculatedMatchCount()
    {
        _repo.Setup(r => r.Count).Returns(10);
        _generator.Setup(g => g.CalculateMatchCount(10)).Returns(630);

        var result = await _sut.Handle(new GetMatchCountQuery(), CancellationToken.None);

        result.Should().Be(630);
    }
}
