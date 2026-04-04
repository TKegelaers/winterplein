using MediatR;
using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public class GetSeasonsQueryHandler(ISeasonRepository seasonRepository)
    : IRequestHandler<GetSeasonsQuery, List<Season>>
{
    public Task<List<Season>> Handle(GetSeasonsQuery request, CancellationToken cancellationToken)
        => Task.FromResult(seasonRepository.GetAll().ToList());
}
