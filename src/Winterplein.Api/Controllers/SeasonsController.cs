using MediatR;
using Microsoft.AspNetCore.Mvc;
using Winterplein.Application.Mappers;
using Winterplein.Application.Seasons;
using Winterplein.Shared.DTOs;

namespace Winterplein.Api.Controllers;

[ApiController]
[Route("api/seasons")]
public class SeasonsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var seasons = await sender.Send(new GetSeasonsQuery());
        return Ok(seasons.Select(s => s.ToDto()).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var season = await sender.Send(new GetSeasonByIdQuery(id));
        return season == null ? NotFound() : Ok(season.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSeasonRequest request)
    {
        var newId = await sender.Send(new CreateSeasonCommand(
            request.Name, request.StartDate, request.EndDate,
            request.Weekday, request.StartHour, request.EndHour));
        var season = await sender.Send(new GetSeasonByIdQuery(newId));
        return Created($"/api/seasons/{newId}", season!.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateSeasonRequest request)
    {
        var updated = await sender.Send(new UpdateSeasonCommand(
            id, request.Name, request.StartDate, request.EndDate,
            request.Weekday, request.StartHour, request.EndHour));
        if (!updated) return NotFound();
        var season = await sender.Send(new GetSeasonByIdQuery(id));
        return Ok(season!.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await sender.Send(new DeleteSeasonCommand(id));
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id:int}/matchdays")]
    public async Task<IActionResult> GetMatchdays(int id)
    {
        var season = await sender.Send(new GetSeasonByIdQuery(id));
        return season == null ? NotFound() : Ok(season.GetMatchdays());
    }

    [HttpGet("{id:int}/players")]
    public async Task<IActionResult> GetPlayers(int id)
    {
        var players = await sender.Send(new GetSeasonPlayersQuery(id));
        return players == null ? NotFound() : Ok(players);
    }

    [HttpPost("{id:int}/players")]
    public async Task<IActionResult> AddPlayer(int id, AddSeasonPlayerRequest request)
    {
        var season = await sender.Send(new AddSeasonPlayerCommand(id, request.PlayerId));
        return season == null ? NotFound() : Ok(season);
    }

    [HttpDelete("{id:int}/players/{playerId:int}")]
    public async Task<IActionResult> RemovePlayer(int id, int playerId)
    {
        var removed = await sender.Send(new RemoveSeasonPlayerCommand(id, playerId));
        return removed ? NoContent() : NotFound();
    }
}
