using Winterplein.Application.Mappers;
using Winterplein.Domain.Enums;
using Winterplein.UnitTests.Common.Builders;

namespace Winterplein.UnitTests.Application;

public class PlayerMapperTests
{
    [Fact]
    public void ToDto_MapsAllProperties()
    {
        var name = new NameBuilder().WithFirstName("Alice").WithLastName("Smith").Build();
        var player = new PlayerBuilder().WithId(42).WithName(name).WithGender(Gender.Male).Build();

        var dto = player.ToDto();

        dto.Id.Should().Be(42);
        dto.FirstName.Should().Be("Alice");
        dto.LastName.Should().Be("Smith");
        dto.Gender.Should().Be("Male");
    }

    [Fact]
    public void ToDto_MapsGenderToString()
    {
        var player = new PlayerBuilder().WithGender(Gender.Female).Build();

        var dto = player.ToDto();

        dto.Gender.Should().Be("Female");
    }
}
