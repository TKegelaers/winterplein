using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Winterplein.Application.IO.DTOs;

namespace Winterplein.IntegrationTests.Seasons;

public class SeasonScheduleTests : IntegrationTestBase
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

    private async Task<(HttpResponseMessage Response, GenerateScheduleResponse? Body)> GenerateSchedule(int seasonId)
    {
        var response = await Client.PostAsJsonAsync(
            $"/api/seasons/{seasonId}/schedule/generate", new { }, _json);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return (response, null);
        var body = await response.Content.ReadFromJsonAsync<GenerateScheduleResponse>(_json);
        return (response, body);
    }

    [Fact]
    public async Task GenerateSchedule_Returns200_WithPersistedPlannedMatches()
    {
        var season = await CreateSeason();
        await EnrolPlayers(season.Id, 4);

        var (response, body) = await GenerateSchedule(season.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().NotBeNull();
        body!.PlannedMatches.Should().NotBeEmpty();
        body.PlannedCount.Should().BeGreaterThan(0);
        body.PlannedMatches.Should().OnlyContain(m => m.SeasonId == season.Id);
        body.PlannedMatches.Should().OnlyContain(m => m.Id > 0);
    }

    [Fact]
    public async Task GenerateSchedule_IsIdempotent_OnRerun()
    {
        var season = await CreateSeason();
        await EnrolPlayers(season.Id, 4);

        var (firstResponse, first) = await GenerateSchedule(season.Id);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        first.Should().NotBeNull();

        var (secondResponse, second) = await GenerateSchedule(season.Id);

        secondResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        second.Should().NotBeNull();
        // Re-running plans nothing new: the total planned count and the
        // remaining-open count are stable.
        second!.PlannedCount.Should().Be(first!.PlannedCount);
        second.OpenCount.Should().Be(first.OpenCount);
        // The persisted set is unchanged — same matches on the same dates.
        second.PlannedMatches.Should().HaveCount(first.PlannedMatches.Count);
        second.PlannedMatches.Select(m => m.Date)
            .Should().BeEquivalentTo(first.PlannedMatches.Select(m => m.Date));
        second.PlannedMatches.Select(m => m.Id)
            .Should().BeEquivalentTo(first.PlannedMatches.Select(m => m.Id));

        // No duplicate rows accumulated in the database.
        using var scoped = Factory.CreateDbContext();
        var dbCount = scoped.Context.PlannedMatches.Count(pm => pm.SeasonId == season.Id);
        dbCount.Should().Be(first.PlannedMatches.Count);
    }

    [Fact]
    public async Task GenerateSchedule_Returns404_ForUnknownSeason()
    {
        var (response, _) = await GenerateSchedule(99999);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GenerateSchedule_Returns200_EmptyPlan_ForFewerThanFourPlayers()
    {
        var season = await CreateSeason();
        await EnrolPlayers(season.Id, 3);

        var (response, body) = await GenerateSchedule(season.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().NotBeNull();
        body!.PlannedMatches.Should().BeEmpty();
        body.PlannedCount.Should().Be(0);
    }
}
