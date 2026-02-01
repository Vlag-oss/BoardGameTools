using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Services.Passwords;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoardGameTools.Application.Password.Commands
{
    public record ResetPasswordCommand(string Token, string NewPassword, string ConfirmPassword) : IRequest<Unit>;

    public class ResetPasswordCommandHandler(
        IAppDbContext context, 
        IDateTimeProvider dateTimeProvider, 
        IPasswordHasher passwordHasher,
        ILogger<ResetPasswordCommandHandler> logger
        ) : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IAppDbContext _context = context;
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ILogger<ResetPasswordCommandHandler> _logger = logger;

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken ct)
        {
            var passwordResetToken = await _context.PasswordResetToken.SingleOrDefaultAsync(p => p.TokenHash == request.Token, ct);

            if (passwordResetToken == null || passwordResetToken.IsUsed || passwordResetToken.Expires <= _dateTimeProvider.UtcNow)
                throw new ResetPasswordTokenException();

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == passwordResetToken.UserId, ct);

            if(user is null)
            {
                _logger.LogError(
                    "La référence à l'utilisateur {UserId} est manquante pour le password reset token {TokenId}",
                    passwordResetToken.UserId,
                    passwordResetToken.Id
                );

                throw new InvalidOperationException("Données incohérentes : le jeton de réinitialisation fait référence à un utilisateur manquant.");
            }

            var samePassword = _passwordHasher.Verify(request.NewPassword, user.PasswordHash);
            if (samePassword)
                throw new SamePasswordException();

            var hashedPassword = _passwordHasher.Hash(request.NewPassword);

            user.ResetPassword(hashedPassword);
            passwordResetToken.MarkAsUsed();

            await _context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}