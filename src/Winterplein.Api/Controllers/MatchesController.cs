using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Winterplein.Application.Commands.GenerateMatches;
using Winterplein.Application.Queries.GetMatchCount;
using Winterplein.Shared.DTOs;

namespace Winterplein.Api.Controllers;

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
