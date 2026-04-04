using Winterplein.Domain.Entities;

namespace Winterplein.UnitTests.Common.Builders;

public class SeasonBuilder
{
    private int _id = 1;
    private string _name = "Summer 2025";
    private DateOnly _startDate = new(2025, 1, 6);
    private DateOnly _endDate = new(2025, 3, 31);
    private DayOfWeek _weekday = DayOfWeek.Monday;
    private TimeOnly _startHour = new(18, 0);
    private TimeOnly _endHour = new(20, 0);
    private readonly List<Player> _players = [];

    public SeasonBuilder WithId(int id) { _id = id; return this; }
    public SeasonBuilder WithName(string name) { _name = name; return this; }
    public SeasonBuilder WithStartDate(DateOnly startDate) { _startDate = startDate; return this; }
    public SeasonBuilder WithEndDate(DateOnly endDate) { _endDate = endDate; return this; }
    public SeasonBuilder WithWeekday(DayOfWeek weekday) { _weekday = weekday; return this; }
    public SeasonBuilder WithStartHour(TimeOnly startHour) { _startHour = startHour; return this; }
    public SeasonBuilder WithEndHour(TimeOnly endHour) { _endHour = endHour; return this; }
    public SeasonBuilder WithPlayer(Player player) { _players.Add(player); return this; }

    public Season Build()
    {
        var season = new Season(_id, _name, _startDate, _endDate, _weekday, _startHour, _endHour);
        foreach (var player in _players)
            season.AddPlayer(player);
        return season;
    }
}
