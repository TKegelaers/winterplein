using Moq;
using Winterplein.Application.Commands.RemovePlayer;
using Winterplein.Application.Interfaces;

namespace Winterplein.UnitTests.Application.Handlers;

public class RemovePlayerCommandHandlerTests
{
    private readonly Mock<IPlayerRepository> _repo = new();

    [Fact]
    public void Handle_CallsRepoRemoveWithCorrectId()
    {
        RemovePlayerCommandHandler.Handle(new RemovePlayerCommand(42), _repo.Object);

        _repo.Verify(r => r.Remove(42), Times.Once);
    }

    [Fact]
    public void Handle_ThrowsKeyNotFoundException_WhenPlayerNotFound()
    {
        _repo.Setup(r => r.Remove(99)).Throws(new KeyNotFoundException());

        var act = () => RemovePlayerCommandHandler.Handle(new RemovePlayerCommand(99), _repo.Object);

        act.Should().Throw<KeyNotFoundException>();
    }
}
