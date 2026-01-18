using BoardGameTools.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<LibraryGame> LibraryGames { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
