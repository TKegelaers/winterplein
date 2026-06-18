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

    public async Task<bool> DeleteBySeasonAndDateAsync(int seasonId, DateOnly date, CancellationToken ct = default)
    {
        var matches = await _db.PlannedMatches
            .Where(pm => pm.SeasonId == seasonId && pm.Date == date)
            .ToListAsync(ct);

        if (matches.Count == 0)
            return false;

        _db.PlannedMatches.RemoveRange(matches);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task DeleteAllBySeasonAsync(int seasonId, CancellationToken ct = default)
    {
        var matches = await _db.PlannedMatches
            .Where(pm => pm.SeasonId == seasonId)
            .ToListAsync(ct);

        if (matches.Count == 0)
            return;

        _db.PlannedMatches.RemoveRange(matches);
        await _db.SaveChangesAsync(ct);
    }
}
