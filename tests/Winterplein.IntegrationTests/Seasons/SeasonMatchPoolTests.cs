using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Winterplein.Application.IO.DTOs;

namespace Winterplein.IntegrationTests.Seasons;

public class SeasonMatchPoolTests : IntegrationTestBase
{
    private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private async Task<SeasonDto> CreateSeason() =>
        (await (await Client.PostAsJsonAsync("/api/seasons",
            new CreateSeasonRequest("Test",
                new DateOnly(2025, 1, 6), new DateOnly(2025, 12, 31),
                DayOfWeek.Monday, new TimeOnly(18, 0), new TimeOnly(20, 0)), _json))
        .Content.ReadFromJsonAsync<SeasonDto>(_json))!;

    private async Task<PlayerDto> CreatePlayer(string first = "Jan", string last = "Doe") =>
        (await (await Client.PostAsJsonAsync("/api/players",
            new AddPlayerRequest(first, last, GenderDto.Male)))
        .Content.ReadFromJsonAsync<PlayerDto>())!;

    private async Task EnrolPlayers(int seasonId, int count)
    {
        var players = await Task.WhenAll(
            Enumerable.Range(1, count).Select(i => CreatePlayer($"P{i}", "L")));
        foreach (var p in players)
            await Client.PostAsJsonAsync(
                $"/api/seasons/{seasonId}/players", new AddSeasonPlayerRequest(p.Id));
    }

    [Fact]
    public async Task GetMatchPool_Returns200_WithMatches_ForFourOrMorePlayers()
    {
        var season = await CreateSeason();
        await EnrolPlayers(season.Id, 4);

        var response = await Client.GetAsync($"/api/seasons/{season.Id}/match-pool");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pool = await response.Content.ReadFromJsonAsync<GenerateMatchesResponse>(_json);
        pool.Should().NotBeNull();
        pool!.TotalCount.Should().Be(3);
        pool.Matches.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetMatchPool_Returns200_EmptyResponse_ForFewerThanFourPlayers()
    {
        var season = await CreateSeason();
        await EnrolPlayers(season.Id, 3);

        var response = await Client.GetAsync($"/api/seasons/{season.Id}/match-pool");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pool = await response.Content.ReadFromJsonAsync<GenerateMatchesResponse>(_json);
        pool.Should().NotBeNull();
        pool!.Matches.Should().BeEmpty();
        pool.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetMatchPool_Returns404_ForUnknownSeason()
    {
        var response = await Client.GetAsync("/api/seasons/99999/match-pool");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
