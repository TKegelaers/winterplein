using Microsoft.EntityFrameworkCore;
using Winterplein.Application.Ports;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Repositories;

public class EfPlannedMatchRepository : IPlannedMatchRepository
{
    private readonly WinterpleinDbContext _db;

    public EfPlannedMatchRepository(WinterpleinDbContext db) => _db = db;

    public async Task<IReadOnlyList<PlannedMatch>> GetAllBySeasonAsync(int seasonId, CancellationToken ct = default) =>
        await _db.PlannedMatches
            .Where(pm => pm.SeasonId == seasonId)
            .ToListAsync(ct);

    public async Task AddRangeAsync(IEnumerable<PlannedMatch> plannedMatches, CancellationToken ct = default)
    {
        _db.PlannedMatches.AddRange(plannedMatches);
        await _db.SaveChangesAsync(ct);
    }
}
