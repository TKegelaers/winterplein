using System.Net;
using System.Net.Http.Json;
using Winterplein.Shared.DTOs;

namespace Winterplein.IntegrationTests;

public class GetAllPlayersTests : IDisposable
{
    private readonly WinterpleinApiFactory _factory = new();
    private readonly HttpClient _client;

    public GetAllPlayersTests() => _client = _factory.CreateClient();

    public void Dispose() => _factory.Dispose();

    [Fact]
    public async Task Returns200WithEmptyList_WhenNoPlayersExist()
    {
        var response = await _client.GetAsync("/api/players");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var players = await response.Content.ReadFromJsonAsync<List<PlayerDto>>();
        players.Should().BeEmpty();
    }
}

public class AddPlayerTests : IDisposable
{
    private readonly WinterpleinApiFactory _factory = new();
    private readonly HttpClient _client;

    public AddPlayerTests() => _client = _factory.CreateClient();

    public void Dispose() => _factory.Dispose();

    [Fact]
    public async Task Returns201WithPlayerDto_WhenValidRequest()
    {
        var request = new AddPlayerRequest("Jan", "Janssen", GenderDto.Male);

        var response = await _client.PostAsJsonAsync("/api/players", request);

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

        var response = await _client.PostAsJsonAsync("/api/players", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Returns400_WhenGenderIsInvalid()
    {
        var json = """{"firstName":"Jan","lastName":"Janssen","gender":"InvalidGender"}""";
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/players", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

public class DeletePlayerTests : IDisposable
{
    private readonly WinterpleinApiFactory _factory = new();
    private readonly HttpClient _client;

    public DeletePlayerTests() => _client = _factory.CreateClient();

    public void Dispose() => _factory.Dispose();

    [Fact]
    public async Task Returns204_WhenPlayerExists()
    {
        var addResponse = await _client.PostAsJsonAsync("/api/players",
            new AddPlayerRequest("Anna", "Berg", GenderDto.Female));
        var player = await addResponse.Content.ReadFromJsonAsync<PlayerDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/players/{player!.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Returns404_WhenPlayerNotFound()
    {
        var response = await _client.DeleteAsync("/api/players/99999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
