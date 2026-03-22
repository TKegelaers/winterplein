using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;

namespace Winterplein.UnitTests.Domain;

public class PlayerTests
{
    [Fact]
    public void Player_StoresProperties()
    {
        var name = new Name("John", "Doe");
        var player = new Player(1, name, Gender.Female);

        player.Id.Should().Be(1);
        player.Name.Should().Be(name);
        player.Gender.Should().Be(Gender.Female);
    }

    [Fact]
    public void Player_Constructor_ThrowsWhenNameIsNull()
    {
        var act = () => new Player(1, null!, Gender.Male);
        act.Should().Throw<ArgumentNullException>();
    }
}
