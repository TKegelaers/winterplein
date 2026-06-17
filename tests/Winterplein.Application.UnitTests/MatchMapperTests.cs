using Winterplein.Application.Mappers;
using Winterplein.Common.UnitTests.Builders;

namespace Winterplein.Application.UnitTests;

public class MatchMapperTests
{
    [Fact]
    public void ToDto_MapsAllProperties()
    {
        var match = new MatchBuilder().WithId(99).Build();

        var dto = match.ToDto();

        dto.Id.Should().Be(99);
        dto.Team1.Should().BeEquivalentTo(match.Team1.ToDto());
        dto.Team2.Should().BeEquivalentTo(match.Team2.ToDto());
    }
}
