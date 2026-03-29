using Moq;
using Winterplein.Application.Commands.AddPlayer;
using Winterplein.Application.Interfaces;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;
using Winterplein.UnitTests.Common.Builders;

namespace Winterplein.UnitTests.Application.Handlers;

public class AddPlayerCommandHandlerTests
{
    private readonly Mock<IPlayerRepository> _playerRepository = new();
    private readonly AddPlayerCommandHandler _handler;

    public AddPlayerCommandHandlerTests() => _handler = new AddPlayerCommandHandler(_playerRepository.Object);

    [Fact]
    public async Task Handle_ReturnsPlayerDto()
    {
        var player = new PlayerBuilder()
            .WithId(5)
            .WithName(new NameBuilder().WithFirstName("John").WithLastName("Doe").Build())
            .Build();
        _playerRepository.Setup(r => r.Add(It.IsAny<Name>(), It.IsAny<Gender>())).Returns(player);

        var result = await _handler.Handle(new AddPlayerCommand("John", "Doe", "Male"), CancellationToken.None);

        result.Id.Should().Be(5);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task Handle_CallsRepoWithCorrectGender()
    {
        var player = new PlayerBuilder().Build();
        _playerRepository.Setup(r => r.Add(It.IsAny<Name>(), It.IsAny<Gender>())).Returns(player);

        await _handler.Handle(new AddPlayerCommand("Jane", "Doe", "female"), CancellationToken.None);

        _playerRepository.Verify(r => r.Add(It.IsAny<Name>(), Gender.Female), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsArgumentException_WhenGenderInvalid()
    {
        var act = () => _handler.Handle(new AddPlayerCommand("John", "Doe", "InvalidGender"), CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Invalid gender*");
    }
}
