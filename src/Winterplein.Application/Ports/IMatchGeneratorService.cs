using Winterplein.Domain.Entities;

namespace Winterplein.Application.Ports;

public interface IMatchGeneratorService
{
    List<Match> GenerateAllMatches(IReadOnlyList<Player> players);
    int CalculateMatchCount(int playerCount);
}
