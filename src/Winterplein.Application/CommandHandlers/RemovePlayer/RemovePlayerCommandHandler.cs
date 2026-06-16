using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;

namespace Winterplein.Application.CommandHandlers.RemovePlayer;

public static class RemovePlayerCommandHandler
{
    public static async Task Handle(RemovePlayerCommand command, IPlayerRepository repo, CancellationToken ct = default)
    {
        await repo.RemoveAsync(command.Id, ct);
    }
}
