using Winterplein.Domain.ValueObjects;

namespace Winterplein.UnitTests.Domain;

public class NameTests
{
    [Theory]
    [InlineData("", "Doe")]
    [InlineData("  ", "Doe")]
    [InlineData("John", "")]
    [InlineData("John", "   ")]
    public void Name_Throws_WhenFirstOrLastNameIsEmptyOrWhitespace(string first, string last)
    {
        Assert.Throws<ArgumentException>(() => new Name(first, last));
    }

    [Fact]
    public void Name_StoresValidNames()
    {
        var name = new Name("John", "Doe");

        Assert.Equal("John", name.FirstName);
        Assert.Equal("Doe", name.LastName);
    }
}
