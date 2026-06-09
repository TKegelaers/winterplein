using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Commands.RemovePlayer;

public static class RemovePlayerCommandHandler
{
    public static async Task Handle(RemovePlayerCommand command, IPlayerRepository repo, CancellationToken ct = default)
    {
        await repo.RemoveAsync(command.Id, ct);
    }
}
