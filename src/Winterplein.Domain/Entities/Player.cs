using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;

namespace Winterplein.Domain.Entities;

public class Player
{
    public int Id { get; }
    public Name Name { get; }
    public Gender Gender { get; set; }

    public Player(int id, Name name)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
