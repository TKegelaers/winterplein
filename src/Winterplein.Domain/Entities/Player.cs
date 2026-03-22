using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;

namespace Winterplein.Domain.Entities;

public class Player
{
    public int Id { get; }
    public Name Name { get; }
    public Gender Gender { get; }

    public Player(int id, Name name, Gender gender)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Gender = gender;
    }
}
