using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Common.Options;
using BoardGameTools.Application.Services.Tokens;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BoardGameTools.Application.Password.Commands
{
    public record ForgotPasswordCommand(string Email) : IRequest<Unit>;

    public class ForgotPasswordCommandHandler(
        IAppDbContext context, 
        IEmailTemplate emailTemplate, 
        IEmailSender emailSender,
        ITokenService tokenService,
        IOptions<ClientOptions> clientOptions,
        ILogger<ForgotPasswordCommandHandler> logger
        ) : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly IAppDbContext _context = context;
        private readonly IEmailTemplate _emailTemplate = emailTemplate;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ClientOptions _options = clientOptions.Value;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger = logger;

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, ct);

            if (user == null || !user.EmailConfirmed)
                return Unit.Value;

            try
            {
                var token = _tokenService.CreateResetPasswordToken(user);
                _context.PasswordResetToken.Add(token);

                var confirmationLink = $"{_options.BaseUrl}/reset-password?token={Uri.EscapeDataString(token.TokenHash)}";
                var emailContent = _emailTemplate.BuildEmailForgotPassword(confirmationLink);

                await _emailSender.SendAsync(user.Email, "Réinitialisation du mot de passe", emailContent, ct);

                await _context.SaveChangesAsync(ct);
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Echec lors de l'envoi du mail de réinitialisation du mot de passe pour l'usager {UserId} ({Email})",
                    user.Id,
                    user.Email.Value);
            }

            return Unit.Value;
        }
    }
}