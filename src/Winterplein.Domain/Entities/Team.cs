namespace Winterplein.Domain.Entities;

public class Team : IEquatable<Team>
{
    public int Id { get; }
    public Player Player1 { get; }
    public Player Player2 { get; }

    public Team(int id, Player player1, Player player2)
    {
        Id = id;
        Player1 = player1 ?? throw new ArgumentNullException(nameof(player1));
        Player2 = player2 ?? throw new ArgumentNullException(nameof(player2));
    }

    public bool Equals(Team? other)
    {
        if (other is null) return false;
        var thisIds = new HashSet<int> { Player1.Id, Player2.Id };
        var otherIds = new HashSet<int> { other.Player1.Id, other.Player2.Id };
        return thisIds.SetEquals(otherIds);
    }

    public override bool Equals(object? obj) => Equals(obj as Team);

    public override int GetHashCode()
    {
        var ids = new[] { Player1.Id, Player2.Id };
        Array.Sort(ids);
        return HashCode.Combine(ids[0], ids[1]);
    }
}
