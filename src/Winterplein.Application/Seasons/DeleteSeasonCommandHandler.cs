using MediatR;
using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Seasons;

public class DeleteSeasonCommandHandler(ISeasonRepository seasonRepository)
    : IRequestHandler<DeleteSeasonCommand, bool>
{
    public Task<bool> Handle(DeleteSeasonCommand request, CancellationToken cancellationToken)
        => Task.FromResult(seasonRepository.Delete(request.Id));
}
