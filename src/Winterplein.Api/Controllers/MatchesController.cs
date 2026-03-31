using MediatR;
using Microsoft.AspNetCore.Mvc;
using Winterplein.Application.Commands.GenerateMatches;
using Winterplein.Application.Queries.GetMatchCount;
using Winterplein.Shared.DTOs;

namespace Winterplein.Api.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchesController(ISender sender) : ControllerBase
{
    [HttpPost("generate")]
    public async Task<IActionResult> Generate() =>
        StatusCode(StatusCodes.Status201Created, await sender.Send(new GenerateMatchesCommand()));

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await sender.Send(new GetMatchCountQuery());
        return Ok(new MatchCountResponse(count));
    }
}
