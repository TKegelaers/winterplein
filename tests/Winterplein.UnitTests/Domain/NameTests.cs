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
        var act = () => new Name(first, last);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Name_StoresValidNames()
    {
        var name = new Name("John", "Doe");

        name.FirstName.Should().Be("John");
        name.LastName.Should().Be("Doe");
    }
}
