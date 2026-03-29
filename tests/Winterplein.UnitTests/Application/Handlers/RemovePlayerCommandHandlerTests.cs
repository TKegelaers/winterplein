using Moq;
using Winterplein.Application.Commands.RemovePlayer;
using Winterplein.Application.Interfaces;

namespace Winterplein.UnitTests.Application.Handlers;

public class RemovePlayerCommandHandlerTests
{
    private readonly Mock<IPlayerRepository> _repo = new();
    private readonly RemovePlayerCommandHandler _sut;

    public RemovePlayerCommandHandlerTests() => _sut = new RemovePlayerCommandHandler(_repo.Object);

    [Fact]
    public async Task Handle_CallsRepoRemoveWithCorrectId()
    {
        await _sut.Handle(new RemovePlayerCommand(42), CancellationToken.None);

        _repo.Verify(r => r.Remove(42), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsKeyNotFoundException_WhenPlayerNotFound()
    {
        _repo.Setup(r => r.Remove(99)).Throws(new KeyNotFoundException());

        var act = () => _sut.Handle(new RemovePlayerCommand(99), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
