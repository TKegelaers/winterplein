using Winterplein.Domain.Enums;

namespace Winterplein.Domain.Entities;

public class PlannedMatch
{
    public int Id { get; private set; }
    public int SeasonId { get; private set; }
    public DateOnly Date { get; private set; }
    public PlannedTeam Team1 { get; private set; }
    public PlannedTeam Team2 { get; private set; }

    public PlannedMatch(int id, int seasonId, DateOnly date, PlannedTeam team1, PlannedTeam team2)
    {
        if (date == default)
            throw new ArgumentException("Date cannot be the default value.", nameof(date));

        Id = id;
        SeasonId = seasonId;
        Date = date;
        Team1 = team1 ?? throw new ArgumentNullException(nameof(team1));
        Team2 = team2 ?? throw new ArgumentNullException(nameof(team2));
    }

    private PlannedMatch() { Team1 = null!; Team2 = null!; }
}

public class PlannedTeam
{
    public PlannedPlayer Player1 { get; private set; }
    public PlannedPlayer Player2 { get; private set; }

    public PlannedTeam(PlannedPlayer player1, PlannedPlayer player2)
    {
        Player1 = player1 ?? throw new ArgumentNullException(nameof(player1));
        Player2 = player2 ?? throw new ArgumentNullException(nameof(player2));
    }

    private PlannedTeam() { Player1 = null!; Player2 = null!; }
}

public class PlannedPlayer
{
    public int PlayerId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Gender Gender { get; private set; }

    public PlannedPlayer(int playerId, string firstName, string lastName, Gender gender)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("FirstName cannot be empty or whitespace.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("LastName cannot be empty or whitespace.", nameof(lastName));

        PlayerId = playerId;
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
    }

    private PlannedPlayer() { FirstName = null!; LastName = null!; }
}
