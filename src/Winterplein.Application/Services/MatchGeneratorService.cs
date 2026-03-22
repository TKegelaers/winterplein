using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Services;

public class MatchGeneratorService : IMatchGeneratorService
{
    public List<Match> GenerateAllMatches(IReadOnlyList<Player> players)
    {
        var matches = new List<Match>();
        int n = players.Count;

        if (n < 4)
            return matches;

        int matchNumber = 1;
        int teamId = 1;

        for (int i = 0; i < n - 3; i++)
        for (int j = i + 1; j < n - 2; j++)
        for (int k = j + 1; k < n - 1; k++)
        for (int l = k + 1; l < n; l++)
        {
            var a = players[i];
            var b = players[j];
            var c = players[k];
            var d = players[l];

            // AB vs CD
            matches.Add(new Match(matchNumber++,
                new Team(teamId++, a, b),
                new Team(teamId++, c, d)));

            // AC vs BD
            matches.Add(new Match(matchNumber++,
                new Team(teamId++, a, c),
                new Team(teamId++, b, d)));

            // AD vs BC
            matches.Add(new Match(matchNumber++,
                new Team(teamId++, a, d),
                new Team(teamId++, b, c)));
        }

        return matches;
    }

    public int CalculateMatchCount(int playerCount)
    {
        if (playerCount < 4)
            return 0;

        // C(N,4) * 3
        int n = playerCount;
        return n * (n - 1) * (n - 2) * (n - 3) / 24 * 3;
    }
}
