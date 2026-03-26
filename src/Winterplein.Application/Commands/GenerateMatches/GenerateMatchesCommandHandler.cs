using MediatR;
using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Commands.GenerateMatches;

public class GenerateMatchesCommandHandler(IPlayerRepository repo, IMatchGeneratorService generator)
    : IRequestHandler<GenerateMatchesCommand, GenerateMatchesResponse>
{
    public Task<GenerateMatchesResponse> Handle(GenerateMatchesCommand request, CancellationToken cancellationToken)
    {
        var allPlayers = repo.GetAll();
        var generated = generator.GenerateAllMatches(allPlayers);
        return Task.FromResult(new GenerateMatchesResponse(
            generated.Select(m => m.ToDto()).ToList(),
            generated.Count));
    }
}
