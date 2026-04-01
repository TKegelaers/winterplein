using System.Net.Http.Json;
using Winterplein.Shared.DTOs;

namespace Winterplein.Client.Services;

public class MatchApiClient(HttpClient httpClient)
{
    public async Task<GenerateMatchesResponse> GenerateMatchesAsync()
    {
        var response = await httpClient.PostAsync("/api/matches/generate", null);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<GenerateMatchesResponse>();
        return result ?? new GenerateMatchesResponse([], 0);
    }

    public async Task<int> GetMatchCountAsync()
    {
        var result = await httpClient.GetFromJsonAsync<MatchCountResponse>("/api/matches/count");
        return result?.Count ?? 0;
    }
}
