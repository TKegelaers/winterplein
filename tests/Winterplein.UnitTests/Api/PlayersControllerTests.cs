using MediatR;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Winterplein.Api.Controllers;
using Winterplein.Application.Commands.AddPlayer;
using Winterplein.Application.Commands.RemovePlayer;
using Winterplein.Application.Queries.GetAllPlayers;
using Winterplein.Shared.DTOs;

namespace Winterplein.UnitTests.Api;

public class PlayersControllerTests
{
    private readonly Mock<ISender> _sender = new();
    private readonly PlayersController _sut;

    public PlayersControllerTests() => _sut = new PlayersController(_sender.Object);

    [Fact]
    public async Task GetAll_ReturnsOkWithPlayers()
    {
        var players = new List<PlayerDto> { new(1, "John", "Doe", "Male") };
        _sender.Setup(s => s.Send(It.IsAny<GetAllPlayersQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(players);

        var result = await _sut.GetAll();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().Be(players);
    }

    [Fact]
    public async Task Add_ReturnsCreatedWithPlayer()
    {
        var dto = new PlayerDto(5, "Jane", "Doe", "Female");
        _sender.Setup(s => s.Send(It.IsAny<AddPlayerCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(dto);

        var result = await _sut.Add(new AddPlayerRequest("Jane", "Doe", "Female"));

        var created = result.Should().BeOfType<CreatedResult>().Subject;
        created.Location.Should().Be("/api/players/5");
        created.Value.Should().Be(dto);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        var result = await _sut.Delete(1);

        result.Should().BeOfType<NoContentResult>();
        _sender.Verify(s => s.Send(It.Is<RemovePlayerCommand>(c => c.Id == 1), It.IsAny<CancellationToken>()), Times.Once);
    }
}
