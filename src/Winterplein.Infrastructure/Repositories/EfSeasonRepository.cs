using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Winterplein.Application.Ports;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Repositories;

public class EfSeasonRepository : ISeasonRepository
{
    private static readonly FieldInfo PlayersField =
        typeof(Season).GetField("_players", BindingFlags.NonPublic | BindingFlags.Instance)!;

    private readonly WinterpleinDbContext _db;

    public EfSeasonRepository(WinterpleinDbContext db) => _db = db;

    public async Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Seasons.Include(s => s.Players).ToListAsync(ct);

    public async Task<Season?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _db.Seasons.Include(s => s.Players).FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<Season> AddAsync(Season season, CancellationToken ct = default)
    {
        _db.Seasons.Add(season);
        await _db.SaveChangesAsync(ct);
        return season;
    }

    public async Task<Season> UpdateAsync(Season season, CancellationToken ct = default)
    {
        var existing = await _db.Seasons
            .Include(s => s.Players)
            .FirstOrDefaultAsync(s => s.Id == season.Id, ct)
            ?? throw new KeyNotFoundException($"Season {season.Id} not found.");

        _db.Entry(existing).CurrentValues.SetValues(season);

        var desiredIds = season.Players.Select(p => p.Id).ToHashSet();
        var currentIds = existing.Players.Select(p => p.Id).ToHashSet();

        // Access backing field directly to bypass domain constraints;
        // EF snapshot-based change detection picks up the modifications.
        var playersList = (List<Player>)PlayersField.GetValue(existing)!;
        playersList.RemoveAll(p => !desiredIds.Contains(p.Id));

        foreach (var p in season.Players.Where(p => !currentIds.Contains(p.Id)))
        {
            var tracked = await _db.Players.FindAsync([p.Id], ct) ?? p;
            playersList.Add(tracked);
        }

        await _db.SaveChangesAsync(ct);
        return existing;
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var season = await _db.Seasons.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"Season {id} not found.");
        _db.Seasons.Remove(season);
        await _db.SaveChangesAsync(ct);
    }
}
