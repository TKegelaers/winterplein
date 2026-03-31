using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Winterplein.Application.Interfaces;
using Winterplein.Infrastructure.Persistence;

namespace Winterplein.IntegrationTests;

public class WinterpleinApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the singleton from Program.cs with a fresh instance per factory,
            // so each test class gets an isolated, empty repository.
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IPlayerRepository));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
        });
    }
}
