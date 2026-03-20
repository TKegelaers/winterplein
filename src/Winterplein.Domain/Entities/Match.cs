namespace Winterplein.Domain.Entities;

public class Match
{
    public int Id { get; set; }
    public Team Team1 { get; set; } = null!;
    public Team Team2 { get; set; } = null!;
}
