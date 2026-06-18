using Winterplein.Application.Ports;
using Winterplein.Application.Mappers;
using Winterplein.Application.IO.Commands;
using Winterplein.Application.IO.DTOs;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.CommandHandlers.GenerateSchedule;

public static class GenerateScheduleCommandHandler
{
    public static async Task<GenerateScheduleResponse?> Handle(
        GenerateScheduleCommand command,
        ISeasonRepository seasonRepository,
        IMatchGeneratorService generator,
        IPlannedMatchRepository plannedMatchRepository,
        CancellationToken ct = default)
    {
        var season = await seasonRepository.GetByIdAsync(command.SeasonId, ct);
        if (season == null)
            return null;

        var existing = await plannedMatchRepository.GetAllBySeasonAsync(command.SeasonId, ct);

        var plannedDates = existing.Select(p => p.Date).ToHashSet();
        var openMatchdays = season.GetMatchdays()
            .Where(d => !plannedDates.Contains(d))
            .ToList();

        var plannedCompositions = existing.Select(CompositionKey).ToHashSet();

        var candidates = generator.GenerateAllMatches(season.Players)
            .Where(m => !plannedCompositions.Contains(CompositionKey(m)))
            .ToList();

        var newPlannedMatches = AssignMatches(openMatchdays, candidates, command.SeasonId);

        if (newPlannedMatches.Count > 0)
            await plannedMatchRepository.AddRangeAsync(newPlannedMatches, ct);

        var allPlanned = existing.Concat(newPlannedMatches).ToList();
        var openCount = openMatchdays.Count - newPlannedMatches.Count;

        var dtos = allPlanned.Select(p => p.ToDto()).ToList();
        return new GenerateScheduleResponse(dtos, allPlanned.Count, openCount);
    }

    private static List<PlannedMatch> AssignMatches(
        IReadOnlyList<DateOnly> openMatchdays,
        List<Match> candidates,
        int seasonId)
    {
        var result = new List<PlannedMatch>();
        var assignable = Math.Min(openMatchdays.Count, candidates.Count);
        if (assignable == 0)
            return result;

        // Fisher-Yates shuffle of candidate indices using the shared thread-safe RNG.
        var indices = Enumerable.Range(0, candidates.Count).ToArray();
        for (var i = indices.Length - 1; i > 0; i--)
        {
            var j = Random.Shared.Next(i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        for (var i = 0; i < assignable; i++)
        {
            var match = candidates[indices[i]];
            result.Add(match.ToSnapshot(seasonId, openMatchdays[i]));
        }

        return result;
    }

    private static (long, long) CompositionKey(Match match)
        => CompositionKey(
            match.Team1.Player1.Id, match.Team1.Player2.Id,
            match.Team2.Player1.Id, match.Team2.Player2.Id);

    private static (long, long) CompositionKey(PlannedMatch match)
        => CompositionKey(
            match.Team1.Player1.PlayerId, match.Team1.Player2.PlayerId,
            match.Team2.Player1.PlayerId, match.Team2.Player2.PlayerId);

    // Normalized composition key: each team is an unordered pair (sorted),
    // and the two teams themselves are unordered (sorted by their encoded pair).
    private static (long, long) CompositionKey(int a1, int a2, int b1, int b2)
    {
        var team1 = EncodePair(a1, a2);
        var team2 = EncodePair(b1, b2);
        return team1 <= team2 ? (team1, team2) : (team2, team1);
    }

    private static long EncodePair(int x, int y)
    {
        var lo = Math.Min(x, y);
        var hi = Math.Max(x, y);
        return ((long)lo << 32) | (uint)hi;
    }
}
