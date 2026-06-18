using Moq;
using Winterplein.Application.Ports;
using Winterplein.Application.IO.Queries;
using Winterplein.Application.QueryHandlers.GetSeasonMatchPool;
using Winterplein.Application.Services;
using Winterplein.Domain.Entities;
using Winterplein.Common.UnitTests.Builders;

namespace Winterplein.Application.UnitTests.Seasons;

public class SeasonMatchPoolHandlerTests
{
    private readonly Mock<ISeasonRepository> _repo = new();
    private readonly MatchGeneratorService _generator = new();

    [Fact]
    public async Task GetSeasonMatchPoolQueryHandler_ReturnsMatches_ForFourOrMorePlayers()
    {
        var builder = new SeasonBuilder().WithId(1);
        for (var i = 1; i <= 4; i++)
            builder.WithPlayer(new PlayerBuilder().WithId(i).Build());
        var season = builder.Build();
        _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(season);

        var result = await GetSeasonMatchPoolQueryHandler.Handle(new GetSeasonMatchPoolQuery(1), _repo.Object, _generator);

        result.Should().NotBeNull();
        result!.TotalCount.Should().Be(3);
        result.Matches.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetSeasonMatchPoolQueryHandler_ReturnsEmptyResponse_ForFewerThanFourPlayers()
    {
        var builder = new SeasonBuilder().WithId(1);
        for (var i = 1; i <= 3; i++)
            builder.WithPlayer(new PlayerBuilder().WithId(i).Build());
        var season = builder.Build();
        _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(season);

        var result = await GetSeasonMatchPoolQueryHandler.Handle(new GetSeasonMatchPoolQuery(1), _repo.Object, _generator);

        result.Should().NotBeNull();
        result!.TotalCount.Should().Be(0);
        result.Matches.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSeasonMatchPoolQueryHandler_ReturnsNull_ForUnknownSeason()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Season?)null);

        var result = await GetSeasonMatchPoolQueryHandler.Handle(new GetSeasonMatchPoolQuery(999), _repo.Object, _generator);

        result.Should().BeNull();
    }
}
