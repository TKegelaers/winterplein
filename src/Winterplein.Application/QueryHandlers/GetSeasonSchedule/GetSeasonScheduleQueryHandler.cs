using Winterplein.Application.Ports;
using Winterplein.Application.Mappers;
using Winterplein.Application.IO.Queries;
using Winterplein.Application.IO.DTOs;

namespace Winterplein.Application.QueryHandlers.GetSeasonSchedule;

public static class GetSeasonScheduleQueryHandler
{
    public static async Task<SeasonScheduleResponse?> Handle(
        GetSeasonScheduleQuery query,
        ISeasonRepository seasonRepository,
        IPlannedMatchRepository plannedMatchRepository,
        CancellationToken ct = default)
    {
        var season = await seasonRepository.GetByIdAsync(query.SeasonId, ct);
        if (season == null)
            return null;

        var planned = await plannedMatchRepository.GetAllBySeasonAsync(query.SeasonId, ct);
        var plannedByDate = planned.ToDictionary(p => p.Date);

        var entries = season.GetMatchdays()
            .OrderBy(d => d)
            .Select(date =>
            {
                var match = plannedByDate.GetValueOrDefault(date);
                return new MatchdayScheduleEntryDto(date, match?.ToDto(), IsPlanned: match != null);
            })
            .ToList();

        return new SeasonScheduleResponse(entries);
    }
}
