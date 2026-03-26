using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Application.Services;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;
using Winterplein.Infrastructure.Persistence;
using Winterplein.Shared.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
builder.Services.AddSingleton<IMatchGeneratorService, MatchGeneratorService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
        policy.WithOrigins("http://localhost:5149", "http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowClient");
app.UseHttpsRedirection();

var players = app.MapGroup("/api/players");

players.MapGet("/", (IPlayerRepository repo) =>
    TypedResults.Ok(repo.GetAll().Select(p => p.ToDto()).ToList()));

players.MapPost("/", (AddPlayerRequest request, IPlayerRepository repo) =>
{
    if (!Enum.TryParse<Gender>(request.Gender, ignoreCase: true, out var gender))
        return Results.BadRequest($"Invalid gender '{request.Gender}'. Valid values: Male, Female.");

    Name name;
    try { name = new Name(request.FirstName, request.LastName); }
    catch (ArgumentException ex) { return Results.BadRequest(ex.Message); }

    var player = repo.Add(name, gender);
    return Results.Created($"/api/players/{player.Id}", player.ToDto());
});

players.MapDelete("/{id:int}", (int id, IPlayerRepository repo) =>
{
    try
    {
        repo.Remove(id);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
});

var matches = app.MapGroup("/api/matches");

matches.MapPost("/generate", (IPlayerRepository repo, IMatchGeneratorService generator) =>
{
    var allPlayers = repo.GetAll();
    var generated = generator.GenerateAllMatches(allPlayers);
    return TypedResults.Ok(new GenerateMatchesResponse(
        generated.Select(m => m.ToDto()).ToList(),
        generated.Count));
});

matches.MapGet("/count", (IPlayerRepository repo, IMatchGeneratorService generator) =>
{
    var count = generator.CalculateMatchCount(repo.Count);
    return TypedResults.Ok(new { count });
});

app.Run();
