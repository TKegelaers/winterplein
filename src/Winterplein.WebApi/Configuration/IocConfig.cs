using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Wolverine;
using Winterplein.Application;
using Winterplein.Application.Ports;
using Winterplein.Application.Services;
using Winterplein.Infrastructure;
using Winterplein.Infrastructure.Repositories;
using Winterplein.WebApi.ExceptionHandling;

namespace Winterplein.WebApi.Configuration;

/// <summary>
/// Centralises the host's IoC and startup wiring (controllers, Swagger,
/// exception handling, Wolverine, EF Core, repositories, services, CORS) so
/// that <c>Program.cs</c> stays thin. Behaviour is identical to the previous
/// inline registrations.
/// </summary>
public static class IocConfig
{
    /// <summary>The CORS policy name used by the Blazor client (http://localhost:5149).</summary>
    private const string CorsPolicyName = "AllowClient";

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        builder.Services.AddSwaggerGen();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Host.UseWolverine(opts => opts.Discovery.IncludeAssembly(typeof(IAmApplication).Assembly));
        builder.Services.AddDbContext<WinterpleinDbContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("WinterpleinDb")));
        builder.Services.AddScoped<IPlayerRepository, EfPlayerRepository>();
        builder.Services.AddScoped<ISeasonRepository, EfSeasonRepository>();
        builder.Services.AddSingleton<IMatchGeneratorService, MatchGeneratorService>();

        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader());
        });

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors(CorsPolicyName);
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.MapControllers();

        return app;
    }
}
