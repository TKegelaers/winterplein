using MediatR;
using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Queries.GetMatchCount;

public class GetMatchCountQueryHandler(IPlayerRepository repo, IMatchGeneratorService generator)
    : IRequestHandler<GetMatchCountQuery, int>
{
    public Task<int> Handle(GetMatchCountQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(generator.CalculateMatchCount(repo.Count));
}
