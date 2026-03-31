using System.Net.Http.Json;
using Winterplein.Shared.DTOs;

namespace Winterplein.Client.Services;

public class PlayerApiClient(HttpClient httpClient)
{
    public async Task<List<PlayerDto>> GetPlayersAsync()
    {
        var result = await httpClient.GetFromJsonAsync<List<PlayerDto>>("/api/players");
        return result ?? [];
    }

    public async Task AddPlayerAsync(AddPlayerRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/api/players", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemovePlayerAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"/api/players/{id}");
        response.EnsureSuccessStatusCode();
    }
}
