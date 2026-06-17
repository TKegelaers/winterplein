using Microsoft.EntityFrameworkCore;
using Winterplein.Domain.Entities;
using Winterplein.Infrastructure;

namespace Winterplein.IntegrationTests.SeedBuilders;

/// <summary>
/// Fluent builder that persists a <see cref="Season"/> through
/// <see cref="WinterpleinDbContext"/>. Construct via the public constructor with
/// Id = 0 so SQL Server identity assigns the generated Id, which flows back on the
/// returned model after <see cref="Seed"/>. Players passed to
/// <see cref="WithPlayers"/> must already be persisted; they are attached so the
/// <c>SeasonPlayers</c> join rows are written without re-inserting the players.
/// </summary>
public class SeasonSeedBuilder
{
    private string _name = "Winter 2025";
    private DateOnly _startDate = new(2025, 1, 6);
    private DateOnly _endDate = new(2025, 3, 31);
    private DayOfWeek _weekday = DayOfWeek.Monday;
    private TimeOnly _startHour = new(18, 0);
    private TimeOnly _endHour = new(20, 0);
    private readonly List<Player> _players = [];

    public SeasonSeedBuilder WithName(string name) { _name = name; return this; }
    public SeasonSeedBuilder WithStartDate(DateOnly startDate) { _startDate = startDate; return this; }
    public SeasonSeedBuilder WithEndDate(DateOnly endDate) { _endDate = endDate; return this; }
    public SeasonSeedBuilder WithWeekday(DayOfWeek weekday) { _weekday = weekday; return this; }
    public SeasonSeedBuilder WithStartHour(TimeOnly startHour) { _startHour = startHour; return this; }
    public SeasonSeedBuilder WithEndHour(TimeOnly endHour) { _endHour = endHour; return this; }

    public SeasonSeedBuilder WithPlayers(IEnumerable<Player> players)
    {
        _players.AddRange(players);
        return this;
    }

    public async Task<Season> Seed(WinterpleinDbContext dbContext)
    {
        // Attach already-persisted players as Unchanged so the join rows are
        // written without EF attempting to re-insert the players themselves.
        foreach (var player in _players)
        {
            var entry = dbContext.Entry(player);
            if (entry.State == EntityState.Detached)
                entry.State = EntityState.Unchanged;
        }

        var season = new Season(0, _name, _startDate, _endDate, _weekday, _startHour, _endHour, _players);
        dbContext.Add(season);
        await dbContext.SaveChangesAsync();
        return season;
    }
}
