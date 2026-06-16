namespace Winterplein.Domain.Entities;

public class Match
{
    public int Id { get; private set; }
    public Team Team1 { get; private set; }
    public Team Team2 { get; private set; }

    public Match(int id, Team team1, Team team2)
    {
        Id = id;
        Team1 = team1 ?? throw new ArgumentNullException(nameof(team1));
        Team2 = team2 ?? throw new ArgumentNullException(nameof(team2));
    }

    private Match() { Team1 = null!; Team2 = null!; }
}
