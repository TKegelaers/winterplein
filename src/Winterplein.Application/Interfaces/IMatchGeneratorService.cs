using Winterplein.Domain.Entities;

namespace Winterplein.Application.Interfaces;

public interface IMatchGeneratorService
{
    List<Match> GenerateAllMatches(IReadOnlyList<Player> players);
    int CalculateMatchCount(int playerCount);
}
