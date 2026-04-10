using Moq;
using Winterplein.Application.Interfaces;
using Winterplein.Application.Queries.GetMatchCount;
using Winterplein.Shared.DTOs;

namespace Winterplein.UnitTests.Application.Handlers;

public class GetMatchCountQueryHandlerTests
{
    private readonly Mock<IPlayerRepository> _repo = new();
    private readonly Mock<IMatchGeneratorService> _generator = new();

    [Fact]
    public void Handle_ReturnsCalculatedMatchCount()
    {
        _repo.Setup(r => r.Count).Returns(10);
        _generator.Setup(g => g.CalculateMatchCount(10)).Returns(630);

        var result = GetMatchCountQueryHandler.Handle(new GetMatchCountQuery(), _repo.Object, _generator.Object);

        result.Should().BeOfType<MatchCountResponse>();
        result.Count.Should().Be(630);
    }
}
