using MediatR;
using Microsoft.AspNetCore.Mvc;
using Winterplein.Application.Commands.AddPlayer;
using Winterplein.Application.Commands.RemovePlayer;
using Winterplein.Application.Queries.GetAllPlayers;
using Winterplein.Shared.DTOs;

namespace Winterplein.Api.Controllers;

[ApiController]
[Route("api/players")]
public class PlayersController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await sender.Send(new GetAllPlayersQuery()));

    [HttpPost]
    public async Task<IActionResult> Add(AddPlayerRequest request)
    {
        var player = await sender.Send(new AddPlayerCommand(request.FirstName, request.LastName, request.Gender));
        return Created($"/api/players/{player.Id}", player);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await sender.Send(new RemovePlayerCommand(id));
        return NoContent();
    }
}
