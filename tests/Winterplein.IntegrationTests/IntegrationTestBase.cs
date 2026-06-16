using Microsoft.Data.SqlClient;
using Respawn;
using Respawn.Graph;

namespace Winterplein.IntegrationTests;

/// <summary>
/// Base class for integration tests that run against the shared SQL Server
/// <c>Winterplein_integrationTests</c> database. Each test gets its own factory
/// and <see cref="HttpClient"/>, and the database is cleared (via Respawn) before
/// the test body runs — the previous run's data is left behind for inspection.
/// </summary>
public abstract class IntegrationTestBase : IAsyncLifetime
{
    private static readonly RespawnerOptions RespawnerOptions = new()
    {
        DbAdapter = DbAdapter.SqlServer,
        TablesToIgnore = [new Table("__EFMigrationsHistory")]
    };

    protected WinterpleinApiFactory Factory { get; } = new();

    protected HttpClient Client { get; }

    protected IntegrationTestBase() => Client = Factory.CreateClient();

    public async Task InitializeAsync()
    {
        // CreateClient forces host creation (and thus migrations) before we reset,
        // guaranteeing the schema exists when Respawn inspects the database.
        await using var connection = new SqlConnection(Factory.ConnectionString);
        await connection.OpenAsync();

        var respawner = await Respawner.CreateAsync(connection, RespawnerOptions);
        await respawner.ResetAsync(connection);
    }

    public async Task DisposeAsync() => await Factory.DisposeAsync();
}
