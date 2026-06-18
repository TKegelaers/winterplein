using Winterplein.Domain.Entities;

namespace Winterplein.Application.Ports;

public interface IPlannedMatchRepository
{
    Task<IReadOnlyList<PlannedMatch>> GetAllBySeasonAsync(int seasonId, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<PlannedMatch> plannedMatches, CancellationToken ct = default);
}
