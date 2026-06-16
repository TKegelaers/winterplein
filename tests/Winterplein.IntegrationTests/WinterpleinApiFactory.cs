using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Winterplein.Infrastructure;

namespace Winterplein.IntegrationTests;

public class WinterpleinApiFactory : WebApplicationFactory<Program>
{
    public WinterpleinApiFactory()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        ConnectionString = configuration.GetConnectionString("WinterpleinDb")
            ?? throw new InvalidOperationException(
                "Connection string 'WinterpleinDb' is missing from the test appsettings.json.");
    }

    /// <summary>
    /// The SQL Server connection string the test database runs against. Exposed so
    /// Respawn can reset the database and seed builders can open their own scope.
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// Creates a fresh <see cref="WinterpleinDbContext"/> in its own DI scope for
    /// seeding and direct database access. The returned <see cref="ScopedDbContext"/>
    /// owns both the context and the scope; disposing it disposes both, so callers
    /// should wrap the result in a <c>using</c>.
    /// </summary>
    public ScopedDbContext CreateDbContext()
    {
        var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WinterpleinDbContext>();
        return new ScopedDbContext(scope, db);
    }

    /// <summary>
    /// A <see cref="WinterpleinDbContext"/> together with the DI scope it was
    /// resolved from. Disposing this disposes the scope (and therefore the
    /// scoped context), avoiding the scope leak that arises from returning a
    /// scoped service without holding onto its scope.
    /// </summary>
    public sealed class ScopedDbContext(IServiceScope scope, WinterpleinDbContext context) : IDisposable
    {
        public WinterpleinDbContext Context { get; } = context;

        public void Dispose() => scope.Dispose();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the SQL Server DbContext registered in Program.cs with one
            // pointed at the integration-test database. Every AddDbContext call
            // registers an IDbContextOptionsConfiguration that is aggregated into
            // the options; the production registration must be removed too,
            // otherwise both end up configured.
            var descriptorsToRemove = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<WinterpleinDbContext>) ||
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType == typeof(WinterpleinDbContext) ||
                (d.ServiceType.IsGenericType &&
                 d.ServiceType.GetGenericTypeDefinition().Name
                     .StartsWith("IDbContextOptionsConfiguration"))).ToList();
            foreach (var descriptor in descriptorsToRemove)
                services.Remove(descriptor);

            services.AddDbContext<WinterpleinDbContext>(opts => opts.UseSqlServer(ConnectionString));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WinterpleinDbContext>();
        db.Database.Migrate();

        return host;
    }
}
