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

    private async Task<(HttpResponseMessage Response, SeasonScheduleResponse? Body)> GetSchedule(int seasonId)
    {
        var response = await Client.GetAsync($"/api/seasons/{seasonId}/schedule");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return (response, null);
        var body = await response.Content.ReadFromJsonAsync<SeasonScheduleResponse>(_json);
        return (response, body);
    }

    private Task<HttpResponseMessage> ClearPlannedMatch(int seasonId, DateOnly date) =>
        Client.DeleteAsync($"/api/seasons/{seasonId}/matchdays/{date:yyyy-MM-dd}/planned-match");

    private Task<HttpResponseMessage> ClearAll(int seasonId) =>
        Client.DeleteAsync($"/api/seasons/{seasonId}/schedule");

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

    [Fact]
    public async Task GetSchedule_Returns200_WithEntryPerMatchday()
    {
        var season = await CreateSeason();

        var matchdays = await Client.GetFromJsonAsync<List<DateOnly>>(
            $"/api/seasons/{season.Id}/matchdays", _json);

        var (response, body) = await GetSchedule(season.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().NotBeNull();
        body!.Entries.Select(e => e.Date).Should().Equal(matchdays!);
        body.Entries.Select(e => e.Date).Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task GetSchedule_MarksPlannedAndOpen_AfterPartialGenerate()
    {
        var season = await CreateSeason();
        await EnrolPlayers(season.Id, 4);

        var (genResponse, gen) = await GenerateSchedule(season.Id);
        genResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        gen.Should().NotBeNull();
        // 4 players → exactly one match can be planned, the rest stay open.
        gen!.PlannedMatches.Should().NotBeEmpty();
        gen.OpenCount.Should().BeGreaterThan(0);

        var (response, body) = await GetSchedule(season.Id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().NotBeNull();

        var plannedDates = gen.PlannedMatches.Select(m => m.Date).ToHashSet();
        body!.Entries.Where(e => e.IsPlanned).Select(e => e.Date)
            .Should().BeEquivalentTo(plannedDates);
        // Planned entries carry their match, open entries do not.
        body.Entries.Where(e => e.IsPlanned)
            .Should().OnlyContain(e => e.PlannedMatch != null && e.PlannedMatch.SeasonId == season.Id);
        body.Entries.Where(e => !e.IsPlanned)
            .Should().OnlyContain(e => e.PlannedMatch == null);
    }

    [Fact]
    public async Task GetSchedule_Returns404_ForUnknownSeason()
    {
        var (response, _) = await GetSchedule(99999);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ClearPlannedMatch_Returns204_AndRemovesMatch()
    {
        var season = await CreateSeason();
        await EnrolPlayers(season.Id, 4);

        var (_, gen) = await GenerateSchedule(season.Id);
        var target = gen!.PlannedMatches.First().Date;

        var response = await ClearPlannedMatch(season.Id, target);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var (_, body) = await GetSchedule(season.Id);
        var entry = body!.Entries.Single(e => e.Date == target);
        entry.IsPlanned.Should().BeFalse();
        entry.PlannedMatch.Should().BeNull();
    }

    [Fact]
    public async Task ClearPlannedMatch_Returns404_WhenNoMatchAtDate()
    {
        var season = await CreateSeason();

        var matchdays = await Client.GetFromJsonAsync<List<DateOnly>>(
            $"/api/seasons/{season.Id}/matchdays", _json);

        // A valid matchday but with no planned match (nothing generated yet).
        var response = await ClearPlannedMatch(season.Id, matchdays!.First());

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ClearPlannedMatch_Returns404_ForUnknownSeason()
    {
        var response = await ClearPlannedMatch(99999, new DateOnly(2025, 1, 6));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ClearAll_Returns204_AndRemovesEveryMatch()
    {
        var season = await CreateSeason();
        await EnrolPlayers(season.Id, 4);
        await GenerateSchedule(season.Id);

        var response = await ClearAll(season.Id);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var (_, body) = await GetSchedule(season.Id);
        body!.Entries.Should().OnlyContain(e => !e.IsPlanned && e.PlannedMatch == null);
    }

    [Fact]
    public async Task ClearAll_Returns204_WhenAlreadyEmpty()
    {
        var season = await CreateSeason();

        var first = await ClearAll(season.Id);
        first.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Idempotent: clearing again on an empty season still succeeds.
        var second = await ClearAll(season.Id);
        second.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ClearAll_Returns404_ForUnknownSeason()
    {
        var response = await ClearAll(99999);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RoundTrip_GenerateClearOneRegenerate_RefillsClearedMatchday()
    {
        var season = await CreateSeason();
        await EnrolPlayers(season.Id, 4);

        var (_, gen) = await GenerateSchedule(season.Id);
        var target = gen!.PlannedMatches.First().Date;

        // Clear that one matchday.
        (await ClearPlannedMatch(season.Id, target)).StatusCode
            .Should().Be(HttpStatusCode.NoContent);

        var (_, afterClear) = await GetSchedule(season.Id);
        afterClear!.Entries.Single(e => e.Date == target).IsPlanned.Should().BeFalse();

        // Re-generate: the cleared matchday is filled again.
        var (regenResponse, _) = await GenerateSchedule(season.Id);
        regenResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var (_, afterRegen) = await GetSchedule(season.Id);
        afterRegen!.Entries.Single(e => e.Date == target).IsPlanned.Should().BeTrue();
    }
}
