using Moq;
using Winterplein.Application.Commands.AddPlayer;
using Winterplein.Application.Interfaces;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;
using Winterplein.Shared.DTOs;
using Winterplein.UnitTests.Common.Builders;

namespace Winterplein.UnitTests.Application.Handlers;

public class AddPlayerCommandHandlerTests
{
    private readonly Mock<IPlayerRepository> _playerRepository = new();

    [Fact]
    public void Handle_ReturnsPlayerDto()
    {
        var player = new PlayerBuilder()
            .WithId(5)
            .WithName(new NameBuilder().WithFirstName("John").WithLastName("Doe").Build())
            .Build();
        _playerRepository.Setup(r => r.Add(It.IsAny<Name>(), It.IsAny<Gender>())).Returns(player);

        var result = AddPlayerCommandHandler.Handle(new AddPlayerCommand("John", "Doe", GenderDto.Male), _playerRepository.Object);

        result.Id.Should().Be(5);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Fact]
    public void Handle_CallsRepoWithCorrectGender()
    {
        var player = new PlayerBuilder().Build();
        _playerRepository.Setup(r => r.Add(It.IsAny<Name>(), It.IsAny<Gender>())).Returns(player);

        AddPlayerCommandHandler.Handle(new AddPlayerCommand("Jane", "Doe", GenderDto.Female), _playerRepository.Object);

        _playerRepository.Verify(r => r.Add(It.IsAny<Name>(), Gender.Female), Times.Once);
    }
}
