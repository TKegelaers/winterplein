using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Persistence;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly List<Player> _players = [];
    private readonly Lock _lock = new();
    private int _nextId = 1;

    public Task<IReadOnlyList<Player>> GetAllAsync(CancellationToken ct = default)
    {
        lock (_lock)
            return Task.FromResult<IReadOnlyList<Player>>(_players.ToList());
    }

    public Task<Player?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        lock (_lock)
            return Task.FromResult(_players.FirstOrDefault(p => p.Id == id));
    }

    public Task<int> CountAsync(CancellationToken ct = default)
    {
        lock (_lock)
            return Task.FromResult(_players.Count);
    }

    public Task<Player> AddAsync(Player player, CancellationToken ct = default)
    {
        lock (_lock)
        {
            var withId = new Player(_nextId++, player.Name, player.Gender);
            _players.Add(withId);
            return Task.FromResult(withId);
        }
    }

    public Task RemoveAsync(int id, CancellationToken ct = default)
    {
        lock (_lock)
        {
            var player = _players.FirstOrDefault(p => p.Id == id)
                ?? throw new KeyNotFoundException($"Player with id {id} not found.");
            _players.Remove(player);
            return Task.CompletedTask;
        }
    }
}
