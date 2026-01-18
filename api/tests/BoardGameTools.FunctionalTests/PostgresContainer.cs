using BoardGameTools.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;

namespace BoardGameTools.FunctionalTests
{
    public class PostgresContainer : ITestDatabase, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _container;
        private Respawner _respawner = null!;

        public string ConnectionString => _container.GetConnectionString();

        public PostgresContainer()
        {
            _container = new PostgreSqlBuilder("postgres:16-alpine")
                .WithDatabase("boardgametools_test")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .WithCleanUp(true)
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();

            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseNpgsql(ConnectionString)
               .Options;

            await using var context = new AppDbContext(options);
            await context.Database.MigrateAsync();

            await using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();

            _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"],
                TablesToIgnore = [new Table("__EFMigrationsHistory")]
            });

        }
        
        public async Task ResetAsync()
        {
            if(_respawner is null)
                throw new InvalidOperationException("Respawner n'est pas initialisé. Appeler InitialAsync() en premier.");

            await using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();

            await _respawner.ResetAsync(conn);
        }

        public async Task DisposeAsync() => await _container.DisposeAsync();
    }
}
