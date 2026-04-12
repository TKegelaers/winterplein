using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Winterplein.Application.Mappers;
using Winterplein.Application.Seasons;
using Winterplein.Domain.Entities;
using Winterplein.Shared.DTOs;

namespace Winterplein.Api.Controllers;

[ApiController]
[Route("api/seasons")]
public class SeasonsController(IMessageBus bus) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var seasons = await bus.InvokeAsync<List<Season>>(new GetSeasonsQuery());
        return Ok(seasons.Select(s => s.ToDto()).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var season = await bus.InvokeAsync<Season?>(new GetSeasonByIdQuery(id));
        return season == null ? NotFound() : Ok(season.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSeasonRequest request)
    {
        var season = await bus.InvokeAsync<Season>(new CreateSeasonCommand(
            request.Name, request.StartDate, request.EndDate,
            request.Weekday, request.StartHour, request.EndHour));
        return Created($"/api/seasons/{season.Id}", season.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateSeasonRequest request)
    {
        var season = await bus.InvokeAsync<Season>(new UpdateSeasonCommand(
            id, request.Name, request.StartDate, request.EndDate,
            request.Weekday, request.StartHour, request.EndHour));
        return Ok(season.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await bus.InvokeAsync(new DeleteSeasonCommand(id));
        return NoContent();
    }

    [HttpGet("{id:int}/matchdays")]
    public async Task<IActionResult> GetMatchdays(int id)
    {
        var season = await bus.InvokeAsync<Season?>(new GetSeasonByIdQuery(id));
        return season == null ? NotFound() : Ok(season.GetMatchdays());
    }

    [HttpGet("{id:int}/players")]
    public async Task<IActionResult> GetPlayers(int id)
    {
        var players = await bus.InvokeAsync<List<Player>?>(new GetSeasonPlayersQuery(id));
        return players == null ? NotFound() : Ok(players.Select(p => p.ToDto()).ToList());
    }

    [HttpPost("{id:int}/players")]
    public async Task<IActionResult> AddPlayer(int id, AddSeasonPlayerRequest request)
    {
        var season = await bus.InvokeAsync<Season>(new AddSeasonPlayerCommand(id, request.PlayerId));
        return Ok(season.ToDto());
    }

    [HttpDelete("{id:int}/players/{playerId:int}")]
    public async Task<IActionResult> RemovePlayer(int id, int playerId)
    {
        await bus.InvokeAsync(new RemoveSeasonPlayerCommand(id, playerId));
        return NoContent();
    }
}
