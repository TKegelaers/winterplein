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
        var player = new Player(name) { Id = 1, Gender = Gender.Female };

        Assert.Equal(1, player.Id);
        Assert.Equal(name, player.Name);
        Assert.Equal(Gender.Female, player.Gender);
    }

    [Fact]
    public void Player_Constructor_ThrowsWhenNameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Player(null!));
    }
}
