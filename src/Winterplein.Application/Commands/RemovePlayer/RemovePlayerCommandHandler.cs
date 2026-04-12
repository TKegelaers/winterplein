using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Commands.RemovePlayer;

public static class RemovePlayerCommandHandler
{
    public static void Handle(RemovePlayerCommand command, IPlayerRepository repo)
    {
        repo.Remove(command.Id);
    }
}
