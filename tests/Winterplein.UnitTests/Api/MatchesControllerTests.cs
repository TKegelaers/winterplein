using Moq;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Winterplein.Api.Controllers;
using Winterplein.Application.Commands.GenerateMatches;
using Winterplein.Application.Queries.GetMatchCount;
using Winterplein.Shared.DTOs;

namespace Winterplein.UnitTests.Api;

public class MatchesControllerTests
{
    private readonly Mock<IMessageBus> _bus = new();
    private readonly MatchesController _sut;

    public MatchesControllerTests() => _sut = new MatchesController(_bus.Object);

    [Fact]
    public async Task Generate_Returns201WithResponse()
    {
        var response = new GenerateMatchesResponse([], 0);
        _bus.Setup(b => b.InvokeAsync<GenerateMatchesResponse>(It.IsAny<GenerateMatchesCommand>(), It.IsAny<CancellationToken>(), It.IsAny<TimeSpan?>())).ReturnsAsync(response);

        var result = await _sut.Generate();

        var created = result.Should().BeOfType<ObjectResult>().Subject;
        created.StatusCode.Should().Be(201);
        created.Value.Should().Be(response);
    }

    [Fact]
    public async Task Count_ReturnsOkWithCount()
    {
        var response = new MatchCountResponse(630);
        _bus.Setup(b => b.InvokeAsync<MatchCountResponse>(It.IsAny<GetMatchCountQuery>(), It.IsAny<CancellationToken>(), It.IsAny<TimeSpan?>())).ReturnsAsync(response);

        var result = await _sut.Count();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().Be(response);
    }
}
