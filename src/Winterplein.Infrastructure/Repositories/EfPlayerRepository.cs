using Microsoft.EntityFrameworkCore;
using Winterplein.Application.Ports;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Repositories;

public class EfPlayerRepository : IPlayerRepository
{
    private readonly WinterpleinDbContext _db;

    public EfPlayerRepository(WinterpleinDbContext db) => _db = db;

    public async Task<IReadOnlyList<Player>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Players.ToListAsync(ct);

    public async Task<Player?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _db.Players.FindAsync([id], ct);

    public async Task<int> CountAsync(CancellationToken ct = default) =>
        await _db.Players.CountAsync(ct);

    public async Task<Player> AddAsync(Player player, CancellationToken ct = default)
    {
        _db.Players.Add(player);
        await _db.SaveChangesAsync(ct);
        return player;
    }

    public async Task RemoveAsync(int id, CancellationToken ct = default)
    {
        var player = await _db.Players.FindAsync([id], ct)
            ?? throw new KeyNotFoundException($"Player with id {id} not found.");
        _db.Players.Remove(player);
        await _db.SaveChangesAsync(ct);
    }
}
