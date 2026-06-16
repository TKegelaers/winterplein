using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Commands.GenerateMatches;

public static class GenerateMatchesCommandHandler
{
    public static async Task<GenerateMatchesResponse> Handle(GenerateMatchesCommand command, IPlayerRepository repo, IMatchGeneratorService generator, CancellationToken ct = default)
    {
        var allPlayers = await repo.GetAllAsync(ct);
        var generated = generator.GenerateAllMatches(allPlayers);
        return new GenerateMatchesResponse(
            generated.Select(m => m.ToDto()).ToList(),
            generated.Count);
    }
}
