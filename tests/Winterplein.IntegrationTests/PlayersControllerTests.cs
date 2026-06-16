using System.Net;
using System.Net.Http.Json;
using Winterplein.Shared.DTOs;

namespace Winterplein.IntegrationTests;

public class GetAllPlayersTests : IntegrationTestBase
{
    [Fact]
    public async Task Returns200WithEmptyList_WhenNoPlayersExist()
    {
        var response = await Client.GetAsync("/api/players");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var players = await response.Content.ReadFromJsonAsync<List<PlayerDto>>();
        players.Should().BeEmpty();
    }
}

public class AddPlayerTests : IntegrationTestBase
{
    [Fact]
    public async Task Returns201WithPlayerDto_WhenValidRequest()
    {
        var request = new AddPlayerRequest("Jan", "Janssen", GenderDto.Male);

        var response = await Client.PostAsJsonAsync("/api/players", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var player = await response.Content.ReadFromJsonAsync<PlayerDto>();
        player.Should().NotBeNull();
        player!.FirstName.Should().Be("Jan");
        player.LastName.Should().Be("Janssen");
    }

    [Theory]
    [InlineData("", "Janssen")]
    [InlineData("   ", "Janssen")]
    [InlineData("Jan", "")]
    [InlineData("Jan", "   ")]
    public async Task Returns400_WhenNameIsBlank(string firstName, string lastName)
    {
        var request = new AddPlayerRequest(firstName, lastName, GenderDto.Male);

        var response = await Client.PostAsJsonAsync("/api/players", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Returns400_WhenGenderIsInvalid()
    {
        var json = """{"firstName":"Jan","lastName":"Janssen","gender":"InvalidGender"}""";
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await Client.PostAsync("/api/players", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

public class DeletePlayerTests : IntegrationTestBase
{
    [Fact]
    public async Task Returns204_WhenPlayerExists()
    {
        var addResponse = await Client.PostAsJsonAsync("/api/players",
            new AddPlayerRequest("Anna", "Berg", GenderDto.Female));
        var player = await addResponse.Content.ReadFromJsonAsync<PlayerDto>();

        var deleteResponse = await Client.DeleteAsync($"/api/players/{player!.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Returns404_WhenPlayerNotFound()
    {
        var response = await Client.DeleteAsync("/api/players/99999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
