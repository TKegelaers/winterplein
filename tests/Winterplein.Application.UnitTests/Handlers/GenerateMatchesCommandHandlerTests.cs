using Moq;
using Winterplein.Application.CommandHandlers.GenerateMatches;
using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;
using Winterplein.Common.UnitTests.Builders;
using Match = Winterplein.Domain.Entities.Match;

namespace Winterplein.Application.UnitTests.Handlers;

public class GenerateMatchesCommandHandlerTests
{
    private readonly Mock<IPlayerRepository> _repo = new();
    private readonly Mock<IMatchGeneratorService> _generator = new();

    [Fact]
    public async Task Handle_ReturnsGeneratedMatches()
    {
        var players = new[] { new PlayerBuilder().WithId(1).Build() };
        var matches = new List<Match> { new MatchBuilder().WithId(1).Build() };
        _repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(players);
        _generator.Setup(g => g.GenerateAllMatches(players)).Returns(matches);

        var result = await GenerateMatchesCommandHandler.Handle(new GenerateMatchesCommand(), _repo.Object, _generator.Object);

        result.TotalCount.Should().Be(1);
        result.Matches.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyResponse_WhenNoMatches()
    {
        _repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Array.Empty<Winterplein.Domain.Entities.Player>());
        _generator.Setup(g => g.GenerateAllMatches(It.IsAny<IReadOnlyList<Winterplein.Domain.Entities.Player>>())).Returns(new List<Match>());

        var result = await GenerateMatchesCommandHandler.Handle(new GenerateMatchesCommand(), _repo.Object, _generator.Object);

        result.TotalCount.Should().Be(0);
        result.Matches.Should().BeEmpty();
    }
}
