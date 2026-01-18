using BoardGameTools.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.FunctionalTests
{
    [Collection("Database collection")]
    public abstract class DatabaseTestBase : IAsyncLifetime
    {
        protected readonly PostgresContainer Db;
        private readonly DbContextOptions<AppDbContext> _options;

        public DatabaseTestBase(PostgresContainer db)
        {
            Db = db;
            _options = new DbContextOptionsBuilder<AppDbContext>()
               .UseNpgsql(Db.ConnectionString)
               .Options;
        }

        protected AppDbContext CreateDbContext() => new(_options);

        protected async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            await using var context = CreateDbContext();
            context.Add(entity);
            await context.SaveChangesAsync();
        }

        public Task InitializeAsync() => Db.ResetAsync();
        public Task DisposeAsync() => Task.CompletedTask;
    }
}
