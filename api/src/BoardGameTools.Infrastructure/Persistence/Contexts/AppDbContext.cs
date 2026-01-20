using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.Infrastructure.Persistence.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<LibraryGame> LibraryGames => Set<LibraryGame>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
