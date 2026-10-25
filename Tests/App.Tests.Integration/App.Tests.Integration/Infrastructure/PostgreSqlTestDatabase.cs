using Testcontainers.PostgreSql;

namespace App.Tests.Integration;

[SetUpFixture]
public sealed class PostgreSqlTestDatabase
{
    private static readonly PostgreSqlContainer Container = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .Build();

    public static string ConnectionString => Container.GetConnectionString();

    [OneTimeSetUp]
    public async Task StartAsync() => await Container.StartAsync();

    [OneTimeTearDown]
    public async Task StopAsync() => await Container.DisposeAsync();
}
