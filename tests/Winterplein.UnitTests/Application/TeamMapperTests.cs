using Winterplein.Application.Mappers;
using Winterplein.UnitTests.Common.Builders;

namespace Winterplein.UnitTests.Application;

public class TeamMapperTests
{
    [Fact]
    public void ToDto_MapsAllProperties()
    {
        var player1 = new PlayerBuilder().WithId(1).Build();
        var player2 = new PlayerBuilder().WithId(2).Build();
        var team = new TeamBuilder().WithId(10).WithPlayer1(player1).WithPlayer2(player2).Build();

        var dto = team.ToDto();

        dto.Id.Should().Be(10);
        dto.Player1.Should().BeEquivalentTo(player1.ToDto());
        dto.Player2.Should().BeEquivalentTo(player2.ToDto());
    }
}
