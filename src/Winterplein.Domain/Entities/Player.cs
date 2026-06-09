using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;

namespace Winterplein.Domain.Entities;

public class Player
{
    public int Id { get; private set; }
    public Name Name { get; private set; }
    public Gender Gender { get; private set; }

    public Player(int id, Name name, Gender gender)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Gender = gender;
    }

    private Player() { Name = null!; }
}
