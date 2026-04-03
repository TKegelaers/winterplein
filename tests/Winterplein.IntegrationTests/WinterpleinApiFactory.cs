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
            // Replace singletons from Program.cs with fresh instances per factory,
            // so each test class gets an isolated, empty repository.
            var playerDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IPlayerRepository));
            if (playerDescriptor != null)
                services.Remove(playerDescriptor);
            services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();

            var seasonDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ISeasonRepository));
            if (seasonDescriptor != null)
                services.Remove(seasonDescriptor);
            services.AddSingleton<ISeasonRepository, InMemorySeasonRepository>();
        });
    }
}
