using MediatR;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Winterplein.Api.Controllers;
using Winterplein.Application.Commands.GenerateMatches;
using Winterplein.Application.Queries.GetMatchCount;
using Winterplein.Shared.DTOs;

namespace Winterplein.UnitTests.Api;

public class MatchesControllerTests
{
    private readonly Mock<ISender> _sender = new();
    private readonly MatchesController _sut;

    public MatchesControllerTests() => _sut = new MatchesController(_sender.Object);

    [Fact]
    public async Task Generate_Returns201WithResponse()
    {
        var response = new GenerateMatchesResponse([], 0);
        _sender.Setup(s => s.Send(It.IsAny<GenerateMatchesCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await _sut.Generate();

        var created = result.Should().BeOfType<ObjectResult>().Subject;
        created.StatusCode.Should().Be(201);
        created.Value.Should().Be(response);
    }

    [Fact]
    public async Task Count_ReturnsOkWithCount()
    {
        _sender.Setup(s => s.Send(It.IsAny<GetMatchCountQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(630);

        var result = await _sut.Count();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().Be(new MatchCountResponse(630));
    }
}
