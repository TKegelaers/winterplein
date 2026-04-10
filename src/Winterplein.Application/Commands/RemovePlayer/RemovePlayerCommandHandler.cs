using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Commands.RemovePlayer;

public static class RemovePlayerCommandHandler
{
    public static void Handle(RemovePlayerCommand request, IPlayerRepository repo)
    {
        repo.Remove(request.Id);
    }
}
