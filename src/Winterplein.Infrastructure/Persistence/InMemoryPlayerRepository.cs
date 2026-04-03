using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;

namespace Winterplein.Infrastructure.Persistence;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly List<Player> _players = [];
    private readonly Lock _lock = new();
    private int _nextId = 1;

    public IReadOnlyList<Player> GetAll()
    {
        lock (_lock)
            return _players.ToList();
    }

    public Player? GetById(int id)
    {
        lock (_lock)
            return _players.FirstOrDefault(p => p.Id == id);
    }

    public int Count
    {
        get { lock (_lock) return _players.Count; }
    }

    public Player Add(Name name, Gender gender)
    {
        lock (_lock)
        {
            var player = new Player(_nextId++, name, gender);
            _players.Add(player);
            return player;
        }
    }

    public void Remove(int id)
    {
        lock (_lock)
        {
            var player = _players.FirstOrDefault(p => p.Id == id)
                ?? throw new KeyNotFoundException($"Player with id {id} not found.");
            _players.Remove(player);
        }
    }
}
