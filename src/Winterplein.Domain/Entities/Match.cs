namespace Winterplein.Domain.Entities;

public class Match
{
    public int Id { get; }
    public Team Team1 { get; }
    public Team Team2 { get; }

    public Match(int id, Team team1, Team team2)
    {
        Id = id;
        Team1 = team1 ?? throw new ArgumentNullException(nameof(team1));
        Team2 = team2 ?? throw new ArgumentNullException(nameof(team2));
    }
}
