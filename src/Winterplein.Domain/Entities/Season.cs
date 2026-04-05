using Winterplein.Domain.Entities;

namespace Winterplein.Domain.Entities;

public class Season
{
    private readonly List<Player> _players = [];

    public int Id { get; }
    public string Name { get; }
    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }
    public DayOfWeek Weekday { get; }
    public TimeOnly StartHour { get; }
    public TimeOnly EndHour { get; }
    public IReadOnlyList<Player> Players => _players.AsReadOnly();

    public Season(int id, string name, DateOnly startDate, DateOnly endDate, DayOfWeek weekday, TimeOnly startHour, TimeOnly endHour, IEnumerable<Player>? players = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (endDate <= startDate)
            throw new ArgumentException("EndDate must be after StartDate.", nameof(endDate));
        if (endHour <= startHour)
            throw new ArgumentException("EndHour must be after StartHour.", nameof(endHour));

        Id = id;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        Weekday = weekday;
        StartHour = startHour;
        EndHour = endHour;

        if (players != null)
            _players.AddRange(players);
    }

    public IReadOnlyList<DateOnly> GetMatchdays()
    {
        var result = new List<DateOnly>();
        var current = StartDate;
        while (current <= EndDate)
        {
            if (current.DayOfWeek == Weekday)
                result.Add(current);
            current = current.AddDays(1);
        }
        return result;
    }

    public void AddPlayer(Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));
        if (_players.Any(p => p.Id == player.Id))
            throw new ArgumentException($"Player with id {player.Id} is already enrolled.", nameof(player));
        _players.Add(player);
    }

    public void RemovePlayer(int playerId)
    {
        var player = _players.FirstOrDefault(p => p.Id == playerId)
            ?? throw new KeyNotFoundException($"Player with id {playerId} is not enrolled.");
        if (_players.Count <= 4)
            throw new ArgumentException("Cannot remove player: season must have at least 4 players.");
        _players.Remove(player);
    }
}
