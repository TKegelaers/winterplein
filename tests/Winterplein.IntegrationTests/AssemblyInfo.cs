using Xunit;

// All test classes share one physical SQL Server database, so the suite must run
// serially. Clearing data before each test (via IntegrationTestBase + Respawn)
// only stays correct when tests do not run in parallel.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
