using Moq;
using Winterplein.Application.Commands.GenerateMatches;
using Winterplein.Application.Interfaces;
using Winterplein.UnitTests.Common.Builders;
using Match = Winterplein.Domain.Entities.Match;

namespace Winterplein.UnitTests.Application.Handlers;

public class GenerateMatchesCommandHandlerTests
{
    private readonly Mock<IPlayerRepository> _repo = new();
    private readonly Mock<IMatchGeneratorService> _generator = new();

    [Fact]
    public void Handle_ReturnsGeneratedMatches()
    {
        var players = new[] { new PlayerBuilder().WithId(1).Build() };
        var matches = new List<Match> { new MatchBuilder().WithId(1).Build() };
        _repo.Setup(r => r.GetAll()).Returns(players);
        _generator.Setup(g => g.GenerateAllMatches(players)).Returns(matches);

        var result = GenerateMatchesCommandHandler.Handle(new GenerateMatchesCommand(), _repo.Object, _generator.Object);

        result.TotalCount.Should().Be(1);
        result.Matches.Should().HaveCount(1);
    }

    [Fact]
    public void Handle_ReturnsEmptyResponse_WhenNoMatches()
    {
        _repo.Setup(r => r.GetAll()).Returns(Array.Empty<Winterplein.Domain.Entities.Player>());
        _generator.Setup(g => g.GenerateAllMatches(It.IsAny<IReadOnlyList<Winterplein.Domain.Entities.Player>>())).Returns(new List<Match>());

        var result = GenerateMatchesCommandHandler.Handle(new GenerateMatchesCommand(), _repo.Object, _generator.Object);

        result.TotalCount.Should().Be(0);
        result.Matches.Should().BeEmpty();
    }
}
