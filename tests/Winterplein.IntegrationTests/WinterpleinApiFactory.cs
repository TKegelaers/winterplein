using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Winterplein.Infrastructure.Persistence;

namespace Winterplein.IntegrationTests;

public class WinterpleinApiFactory : WebApplicationFactory<Program>
{
    // A single open connection keeps the in-memory SQLite database alive for the
    // lifetime of the factory; closing it drops the schema and all data.
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    public WinterpleinApiFactory() => _connection.Open();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the SQL Server DbContext registered in Program.cs with a
            // SQLite in-memory one, so each test class gets an isolated database
            // that exercises the full EF Core pipeline without needing SQL Server.
            // Every AddDbContext call registers an IDbContextOptionsConfiguration
            // that is aggregated into the options; the SQL Server one must be
            // removed too, otherwise both providers end up configured.
            var descriptorsToRemove = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<WinterpleinDbContext>) ||
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType == typeof(WinterpleinDbContext) ||
                (d.ServiceType.IsGenericType &&
                 d.ServiceType.GetGenericTypeDefinition().Name
                     .StartsWith("IDbContextOptionsConfiguration"))).ToList();
            foreach (var descriptor in descriptorsToRemove)
                services.Remove(descriptor);

            services.AddDbContext<WinterpleinDbContext>(opts => opts.UseSqlite(_connection));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WinterpleinDbContext>();
        db.Database.EnsureCreated();

        return host;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _connection.Dispose();
        base.Dispose(disposing);
    }
}
