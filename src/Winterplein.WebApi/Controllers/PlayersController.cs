using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Winterplein.Application.IO.Commands;
using Winterplein.Application.IO.DTOs;
using Winterplein.Application.IO.Queries;

namespace Winterplein.WebApi.Controllers;

[ApiController]
[Route("api/players")]
public class PlayersController(IMessageBus bus) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await bus.InvokeAsync<List<PlayerDto>>(new GetAllPlayersQuery()));

    [HttpPost]
    public async Task<IActionResult> Add(AddPlayerRequest request)
    {
        var player = await bus.InvokeAsync<PlayerDto>(new AddPlayerCommand(request.FirstName, request.LastName, request.Gender));
        return Created($"/api/players/{player.Id}", player);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await bus.InvokeAsync(new RemovePlayerCommand(id));
        return NoContent();
    }
}
