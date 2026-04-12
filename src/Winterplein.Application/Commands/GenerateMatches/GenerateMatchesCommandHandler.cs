using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Commands.GenerateMatches;

public static class GenerateMatchesCommandHandler
{
    public static GenerateMatchesResponse Handle(GenerateMatchesCommand command, IPlayerRepository repo, IMatchGeneratorService generator)
    {
        var allPlayers = repo.GetAll();
        var generated = generator.GenerateAllMatches(allPlayers);
        return new GenerateMatchesResponse(
            generated.Select(m => m.ToDto()).ToList(),
            generated.Count);
    }
}
