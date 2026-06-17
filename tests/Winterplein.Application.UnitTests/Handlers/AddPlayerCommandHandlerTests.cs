using Moq;
using Winterplein.Application.CommandHandlers.AddPlayer;
using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;
using Winterplein.Application.IO.DTOs;
using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Common.UnitTests.Builders;

namespace Winterplein.Application.UnitTests.Handlers;

public class AddPlayerCommandHandlerTests
{
    private readonly Mock<IPlayerRepository> _playerRepository = new();

    [Fact]
    public async Task Handle_ReturnsPlayerDto()
    {
        var player = new PlayerBuilder()
            .WithId(5)
            .WithName(new NameBuilder().WithFirstName("John").WithLastName("Doe").Build())
            .Build();
        _playerRepository.Setup(r => r.AddAsync(It.IsAny<Player>(), It.IsAny<CancellationToken>())).ReturnsAsync(player);

        var result = await AddPlayerCommandHandler.Handle(new AddPlayerCommand("John", "Doe", GenderDto.Male), _playerRepository.Object);

        result.Id.Should().Be(5);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task Handle_CallsRepoWithCorrectGender()
    {
        var player = new PlayerBuilder().Build();
        _playerRepository.Setup(r => r.AddAsync(It.IsAny<Player>(), It.IsAny<CancellationToken>())).ReturnsAsync(player);

        await AddPlayerCommandHandler.Handle(new AddPlayerCommand("Jane", "Doe", GenderDto.Female), _playerRepository.Object);

        _playerRepository.Verify(r => r.AddAsync(It.Is<Player>(p => p.Gender == Gender.Female), It.IsAny<CancellationToken>()), Times.Once);
    }
}
