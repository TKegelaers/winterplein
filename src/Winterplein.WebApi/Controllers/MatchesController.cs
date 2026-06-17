using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Winterplein.Application.IO.Commands;
using Winterplein.Application.IO.DTOs;
using Winterplein.Application.IO.Queries;

namespace Winterplein.WebApi.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchesController(IMessageBus bus) : ControllerBase
{
    [HttpPost("generate")]
    public async Task<IActionResult> Generate() =>
        StatusCode(StatusCodes.Status201Created, await bus.InvokeAsync<GenerateMatchesResponse>(new GenerateMatchesCommand()));

    [HttpGet("count")]
    public async Task<IActionResult> Count() =>
        Ok(await bus.InvokeAsync<MatchCountResponse>(new GetMatchCountQuery()));
}
