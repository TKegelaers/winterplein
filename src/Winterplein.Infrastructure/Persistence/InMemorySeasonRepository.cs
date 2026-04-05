using System.Collections.Concurrent;
using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Persistence;

public class InMemorySeasonRepository : ISeasonRepository
{
    private readonly ConcurrentDictionary<int, Season> _seasons = new();
    private int _nextId = 1;

    public IReadOnlyList<Season> GetAll()
        => _seasons.Values.ToList();

    public Season? GetById(int id)
        => _seasons.TryGetValue(id, out var season) ? season : null;

    public Season Add(Season season)
    {
        var id = Interlocked.Increment(ref _nextId) - 1;
        var withId = new Season(id, season.Name, season.StartDate, season.EndDate,
            season.Weekday, season.StartHour, season.EndHour, season.Players);
        _seasons[id] = withId;
        return withId;
    }

    public bool Update(Season season)
    {
        if (!_seasons.ContainsKey(season.Id))
            return false;
        _seasons[season.Id] = season;
        return true;
    }

    public bool Delete(int id)
        => _seasons.TryRemove(id, out _);
}
