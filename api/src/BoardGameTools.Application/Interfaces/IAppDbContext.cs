using BoardGameTools.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<LibraryGame> LibraryGames { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
