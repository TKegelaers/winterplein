using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;

namespace Winterplein.Domain.Entities;

public class Player
{
    public int Id { get; set; }
    public Name Name { get; }
    public Gender Gender { get; set; }

    public Player(Name name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
