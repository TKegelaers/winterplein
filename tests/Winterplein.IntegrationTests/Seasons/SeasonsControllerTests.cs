using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Winterplein.Shared.DTOs;

namespace Winterplein.IntegrationTests.Seasons;

file static class JsonOpts
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };
}

public class SeasonCrudTests : IDisposable
{
    private readonly WinterpleinApiFactory _factory = new();
    private readonly HttpClient _client;

    public SeasonCrudTests() => _client = _factory.CreateClient();
    public void Dispose() => _factory.Dispose();

    private static CreateSeasonRequest ValidRequest(string name = "Test Season") =>
        new(name, new DateOnly(2025, 1, 6), new DateOnly(2025, 3, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0));

    [Fact]
    public async Task FullCrudCycle()
    {
        // POST → 201
        var createResponse = await _client.PostAsJsonAsync("/api/seasons", ValidRequest(), JsonOpts.Options);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResponse.Content.ReadFromJsonAsync<SeasonDto>(JsonOpts.Options);
        created.Should().NotBeNull();
        created!.Name.Should().Be("Test Season");
        int id = created.Id;

        // GET list → contains season
        var listResponse = await _client.GetAsync("/api/seasons");
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await listResponse.Content.ReadFromJsonAsync<List<SeasonDto>>(JsonOpts.Options);
        list.Should().Contain(s => s.Id == id);

        // GET by id → 200
        var getResponse = await _client.GetAsync($"/api/seasons/{id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var fetched = await getResponse.Content.ReadFromJsonAsync<SeasonDto>(JsonOpts.Options);
        fetched!.Id.Should().Be(id);

        // PUT → 200 with updated name
        var updateRequest = new UpdateSeasonRequest("Updated Season",
            new DateOnly(2025, 1, 6), new DateOnly(2025, 3, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0));
        var putResponse = await _client.PutAsJsonAsync($"/api/seasons/{id}", updateRequest, JsonOpts.Options);
        putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await putResponse.Content.ReadFromJsonAsync<SeasonDto>(JsonOpts.Options);
        updated!.Name.Should().Be("Updated Season");

        // DELETE → 204
        var deleteResponse = await _client.DeleteAsync($"/api/seasons/{id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // GET by id after delete → 404
        var afterDeleteResponse = await _client.GetAsync($"/api/seasons/{id}");
        afterDeleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

public class SeasonValidationTests : IDisposable
{
    private readonly WinterpleinApiFactory _factory = new();
    private readonly HttpClient _client;

    public SeasonValidationTests() => _client = _factory.CreateClient();
    public void Dispose() => _factory.Dispose();

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Post_Returns400_WhenNameIsEmpty(string name)
    {
        var request = new CreateSeasonRequest(name,
            new DateOnly(2025, 1, 6), new DateOnly(2025, 3, 31),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0));

        var response = await _client.PostAsJsonAsync("/api/seasons", request, JsonOpts.Options);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_Returns400_WhenEndDateBeforeStartDate()
    {
        var request = new CreateSeasonRequest("Test",
            new DateOnly(2025, 6, 1), new DateOnly(2025, 1, 1),
            DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0));

        var response = await _client.PostAsJsonAsync("/api/seasons", request, JsonOpts.Options);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_Returns400_WhenEndHourBeforeStartHour()
    {
        var request = new CreateSeasonRequest("Test",
            new DateOnly(2025, 1, 1), new DateOnly(2025, 12, 31),
            DayOfWeek.Monday, new TimeOnly(20, 0), new TimeOnly(18, 0));

        var response = await _client.PostAsJsonAsync("/api/seasons", request, JsonOpts.Options);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

public class SeasonMatchdaysTests : IDisposable
{
    private readonly WinterpleinApiFactory _factory = new();
    private readonly HttpClient _client;

    public SeasonMatchdaysTests() => _client = _factory.CreateClient();
    public void Dispose() => _factory.Dispose();

    [Fact]
    public async Task GetMatchdays_ReturnsCorrectDates()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/seasons",
            new CreateSeasonRequest("Test",
                new DateOnly(2025, 1, 6), new DateOnly(2025, 1, 27),
                DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)), JsonOpts.Options);
        var season = await createResponse.Content.ReadFromJsonAsync<SeasonDto>(JsonOpts.Options);

        var response = await _client.GetAsync($"/api/seasons/{season!.Id}/matchdays");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var matchdays = await response.Content.ReadFromJsonAsync<List<DateOnly>>();
        matchdays.Should().HaveCount(4);
        matchdays.Should().AllSatisfy(d => d.DayOfWeek.Should().Be(DayOfWeek.Monday));
    }

    [Fact]
    public async Task GetMatchdays_Returns404_ForUnknownSeason()
    {
        var response = await _client.GetAsync("/api/seasons/99999/matchdays");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

public class SeasonPlayerTests : IDisposable
{
    private readonly WinterpleinApiFactory _factory = new();
    private readonly HttpClient _client;

    public SeasonPlayerTests() => _client = _factory.CreateClient();
    public void Dispose() => _factory.Dispose();

    private async Task<SeasonDto> CreateSeason() =>
        (await (await _client.PostAsJsonAsync("/api/seasons",
            new CreateSeasonRequest("Test",
                new DateOnly(2025, 1, 6), new DateOnly(2025, 12, 31),
                DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)), JsonOpts.Options))
        .Content.ReadFromJsonAsync<SeasonDto>(JsonOpts.Options))!;

    private async Task<PlayerDto> CreatePlayer(string first = "Jan", string last = "Doe") =>
        (await (await _client.PostAsJsonAsync("/api/players",
            new AddPlayerRequest(first, last, GenderDto.Male)))
        .Content.ReadFromJsonAsync<PlayerDto>())!;

    [Fact]
    public async Task AddPlayer_Returns200_WithUpdatedSeason()
    {
        var season = await CreateSeason();
        var player = await CreatePlayer();

        var response = await _client.PostAsJsonAsync(
            $"/api/seasons/{season.Id}/players", new AddSeasonPlayerRequest(player.Id));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<SeasonDto>(JsonOpts.Options);
        updated!.Players.Should().Contain(p => p.Id == player.Id);
    }

    [Fact]
    public async Task AddPlayer_Returns404_WhenSeasonNotFound()
    {
        var player = await CreatePlayer();

        var response = await _client.PostAsJsonAsync(
            "/api/seasons/99999/players", new AddSeasonPlayerRequest(player.Id));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddPlayer_Returns404_WhenPlayerNotFound()
    {
        var season = await CreateSeason();

        var response = await _client.PostAsJsonAsync(
            $"/api/seasons/{season.Id}/players", new AddSeasonPlayerRequest(99999));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPlayers_Returns200_WithEnrolledPlayers()
    {
        var season = await CreateSeason();
        var player = await CreatePlayer();
        await _client.PostAsJsonAsync(
            $"/api/seasons/{season.Id}/players", new AddSeasonPlayerRequest(player.Id));

        var response = await _client.GetAsync($"/api/seasons/{season.Id}/players");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var players = await response.Content.ReadFromJsonAsync<List<PlayerDto>>();
        players.Should().Contain(p => p.Id == player.Id);
    }

    [Fact]
    public async Task RemovePlayer_Returns204_WhenRemoved()
    {
        var season = await CreateSeason();
        var players = await Task.WhenAll(
            Enumerable.Range(1, 5).Select(i => CreatePlayer($"P{i}", "L")));
        foreach (var p in players)
            await _client.PostAsJsonAsync(
                $"/api/seasons/{season.Id}/players", new AddSeasonPlayerRequest(p.Id));

        var response = await _client.DeleteAsync($"/api/seasons/{season.Id}/players/{players[0].Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task RemovePlayer_Returns404_WhenPlayerNotEnrolled()
    {
        var season = await CreateSeason();

        var response = await _client.DeleteAsync($"/api/seasons/{season.Id}/players/99999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemovePlayer_Returns400_WhenOnlyFourPlayersEnrolled()
    {
        var season = await CreateSeason();
        var players = await Task.WhenAll(
            Enumerable.Range(1, 4).Select(i => CreatePlayer($"P{i}", "L")));
        foreach (var p in players)
            await _client.PostAsJsonAsync(
                $"/api/seasons/{season.Id}/players", new AddSeasonPlayerRequest(p.Id));

        var response = await _client.DeleteAsync($"/api/seasons/{season.Id}/players/{players[0].Id}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
