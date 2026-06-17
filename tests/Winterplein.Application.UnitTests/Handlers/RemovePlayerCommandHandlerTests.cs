using Moq;
using Winterplein.Application.CommandHandlers.RemovePlayer;
using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;

namespace Winterplein.Application.UnitTests.Handlers;

public class RemovePlayerCommandHandlerTests
{
    private readonly Mock<IPlayerRepository> _repo = new();

    [Fact]
    public async Task Handle_CallsRepoRemoveWithCorrectId()
    {
        _repo.Setup(r => r.RemoveAsync(42, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await RemovePlayerCommandHandler.Handle(new RemovePlayerCommand(42), _repo.Object);

        _repo.Verify(r => r.RemoveAsync(42, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsKeyNotFoundException_WhenPlayerNotFound()
    {
        _repo.Setup(r => r.RemoveAsync(99, It.IsAny<CancellationToken>())).ThrowsAsync(new KeyNotFoundException());

        var act = () => RemovePlayerCommandHandler.Handle(new RemovePlayerCommand(99), _repo.Object);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
