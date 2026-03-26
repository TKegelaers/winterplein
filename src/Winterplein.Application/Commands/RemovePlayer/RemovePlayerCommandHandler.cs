using MediatR;
using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Commands.RemovePlayer;

public class RemovePlayerCommandHandler(IPlayerRepository repo)
    : IRequestHandler<RemovePlayerCommand>
{
    public Task Handle(RemovePlayerCommand request, CancellationToken cancellationToken)
    {
        repo.Remove(request.Id);
        return Task.CompletedTask;
    }
}
