using BoardGameTools.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Game> Games { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<PasswordResetToken> PasswordResetToken { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
