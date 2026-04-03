using MediatR;
using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public class GetSeasonByIdQueryHandler(ISeasonRepository seasonRepository)
    : IRequestHandler<GetSeasonByIdQuery, Season?>
{
    public Task<Season?> Handle(GetSeasonByIdQuery request, CancellationToken cancellationToken)
        => Task.FromResult(seasonRepository.GetById(request.Id));
}
