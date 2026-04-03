using System.Net;
using System.Net.Http.Json;
using Winterplein.Shared.DTOs;

namespace Winterplein.Client.Services;

public class SeasonApiClient(HttpClient httpClient)
{
    public async Task<List<SeasonDto>> GetSeasonsAsync()
    {
        var result = await httpClient.GetFromJsonAsync<List<SeasonDto>>("/api/seasons");
        return result ?? [];
    }

    public async Task<SeasonDto?> GetSeasonAsync(int id)
    {
        var response = await httpClient.GetAsync($"/api/seasons/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SeasonDto>();
    }

    public async Task<SeasonDto> CreateSeasonAsync(CreateSeasonRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/api/seasons", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<SeasonDto>())!;
    }

    public async Task<SeasonDto?> UpdateSeasonAsync(int id, UpdateSeasonRequest request)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/seasons/{id}", request);
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SeasonDto>();
    }

    public async Task<bool> DeleteSeasonAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"/api/seasons/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound) return false;
        response.EnsureSuccessStatusCode();
        return true;
    }

    public async Task<List<DateOnly>> GetMatchdaysAsync(int id)
    {
        var result = await httpClient.GetFromJsonAsync<List<DateOnly>>($"/api/seasons/{id}/matchdays");
        return result ?? [];
    }

    public async Task<List<PlayerDto>> GetSeasonPlayersAsync(int id)
    {
        var result = await httpClient.GetFromJsonAsync<List<PlayerDto>>($"/api/seasons/{id}/players");
        return result ?? [];
    }

    public async Task<SeasonDto?> AddPlayerToSeasonAsync(int seasonId, AddSeasonPlayerRequest request)
    {
        var response = await httpClient.PostAsJsonAsync($"/api/seasons/{seasonId}/players", request);
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SeasonDto>();
    }

    public async Task<bool> RemovePlayerFromSeasonAsync(int seasonId, int playerId)
    {
        var response = await httpClient.DeleteAsync($"/api/seasons/{seasonId}/players/{playerId}");
        if (response.StatusCode == HttpStatusCode.NotFound) return false;
        response.EnsureSuccessStatusCode();
        return true;
    }
}
