using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;

namespace Winterplein.UnitTests.Common.Builders;

public class PlayerBuilder
{
    private int _id = 1;
    private Name _name = new NameBuilder().Build();
    private Gender _gender = Gender.Male;

    public PlayerBuilder WithId(int id) { _id = id; return this; }
    public PlayerBuilder WithName(Name name) { _name = name; return this; }
    public PlayerBuilder WithGender(Gender gender) { _gender = gender; return this; }

    public Player Build() => new(_id, _name, _gender);
}
