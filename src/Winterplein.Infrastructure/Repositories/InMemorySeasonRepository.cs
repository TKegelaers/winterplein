using System.Collections.Concurrent;
using Winterplein.Application.Ports;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Repositories;

public class InMemorySeasonRepository : ISeasonRepository
{
    private readonly ConcurrentDictionary<int, Season> _seasons = new();
    private int _nextId = 1;

    public Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Season>>(_seasons.Values.ToList());

    public Task<Season?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_seasons.TryGetValue(id, out var season) ? season : null);

    public Task<Season> AddAsync(Season season, CancellationToken ct = default)
    {
        var id = Interlocked.Increment(ref _nextId) - 1;
        var withId = new Season(id, season.Name, season.StartDate, season.EndDate,
            season.Weekday, season.StartHour, season.EndHour, season.Players);
        _seasons[id] = withId;
        return Task.FromResult(withId);
    }

    public Task<Season> UpdateAsync(Season season, CancellationToken ct = default)
    {
        if (!_seasons.ContainsKey(season.Id))
            throw new KeyNotFoundException($"Season {season.Id} not found.");
        _seasons[season.Id] = season;
        return Task.FromResult(season);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default)
    {
        if (!_seasons.TryRemove(id, out _))
            throw new KeyNotFoundException($"Season {id} not found.");
        return Task.CompletedTask;
    }
}
