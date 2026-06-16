using System.Net;
using System.Net.Http.Json;
using Winterplein.Shared.DTOs;

namespace Winterplein.IntegrationTests;

public class GenerateMatchesTests : IntegrationTestBase
{
    [Fact]
    public async Task Returns201WithCorrectMatchCount_After4PlayersAdded()
    {
        // 4 players → C(4,4) × 3 = 1 × 3 = 3 matches
        await AddPlayerAsync("Alice", "A", GenderDto.Female);
        await AddPlayerAsync("Bob", "B", GenderDto.Male);
        await AddPlayerAsync("Carol", "C", GenderDto.Female);
        await AddPlayerAsync("Dave", "D", GenderDto.Male);

        var response = await Client.PostAsync("/api/matches/generate", null);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<GenerateMatchesResponse>();
        result.Should().NotBeNull();
        result!.TotalCount.Should().Be(3);
        result.Matches.Should().HaveCount(3);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Returns201WithZeroMatches_WhenFewerThan4Players(int playerCount)
    {
        for (int i = 1; i <= playerCount; i++)
            await AddPlayerAsync($"Player{i}", "X", GenderDto.Male);

        var response = await Client.PostAsync("/api/matches/generate", null);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<GenerateMatchesResponse>();
        result!.TotalCount.Should().Be(0);
        result.Matches.Should().BeEmpty();
    }

    private async Task AddPlayerAsync(string first, string last, GenderDto gender) =>
        await Client.PostAsJsonAsync("/api/players", new AddPlayerRequest(first, last, gender));
}

public class GetMatchCountTests : IntegrationTestBase
{
    [Fact]
    public async Task Returns200WithZeroCount_WhenNoPlayersExist()
    {
        var response = await Client.GetAsync("/api/matches/count");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<MatchCountResponse>();
        result.Should().NotBeNull();
        result!.Count.Should().Be(0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Returns200WithZeroCount_WhenFewerThan4Players(int playerCount)
    {
        for (int i = 1; i <= playerCount; i++)
            await Client.PostAsJsonAsync("/api/players", new AddPlayerRequest($"Player{i}", "X", GenderDto.Male));

        var response = await Client.GetAsync("/api/matches/count");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<MatchCountResponse>();
        result!.Count.Should().Be(0);
    }

    [Fact]
    public async Task Returns200WithCorrectCount_After10PlayersAdded()
    {
        // 10 players → C(10,4) × 3 = 210 × 3 = 630 matches
        for (int i = 1; i <= 10; i++)
            await Client.PostAsJsonAsync("/api/players", new AddPlayerRequest($"Player{i}", "X", GenderDto.Male));

        var response = await Client.GetAsync("/api/matches/count");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<MatchCountResponse>();
        result!.Count.Should().Be(630);
    }
}
