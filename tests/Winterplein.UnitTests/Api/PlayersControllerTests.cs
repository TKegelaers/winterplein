using Moq;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Winterplein.Api.Controllers;
using Winterplein.Application.Commands.AddPlayer;
using Winterplein.Application.Commands.RemovePlayer;
using Winterplein.Application.Queries.GetAllPlayers;
using Winterplein.Shared.DTOs;

namespace Winterplein.UnitTests.Api;

public class PlayersControllerTests
{
    private readonly Mock<IMessageBus> _bus = new();
    private readonly PlayersController _sut;

    public PlayersControllerTests() => _sut = new PlayersController(_bus.Object);

    [Fact]
    public async Task GetAll_ReturnsOkWithPlayers()
    {
        var players = new List<PlayerDto> { new(1, "John", "Doe", "Male") };
        _bus.Setup(b => b.InvokeAsync<List<PlayerDto>>(It.IsAny<GetAllPlayersQuery>(), It.IsAny<CancellationToken>(), It.IsAny<TimeSpan?>())).ReturnsAsync(players);

        var result = await _sut.GetAll();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().Be(players);
    }

    [Fact]
    public async Task Add_ReturnsCreatedWithPlayer()
    {
        var dto = new PlayerDto(5, "Jane", "Doe", "Female");
        _bus.Setup(b => b.InvokeAsync<PlayerDto>(It.IsAny<AddPlayerCommand>(), It.IsAny<CancellationToken>(), It.IsAny<TimeSpan?>())).ReturnsAsync(dto);

        var result = await _sut.Add(new AddPlayerRequest("Jane", "Doe", GenderDto.Female));

        var created = result.Should().BeOfType<CreatedResult>().Subject;
        created.Location.Should().Be("/api/players/5");
        created.Value.Should().Be(dto);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        var result = await _sut.Delete(1);

        result.Should().BeOfType<NoContentResult>();
        _bus.Verify(b => b.InvokeAsync(It.Is<RemovePlayerCommand>(c => c.Id == 1), It.IsAny<CancellationToken>(), It.IsAny<TimeSpan?>()), Times.Once);
    }
}
