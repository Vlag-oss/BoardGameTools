using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Services.Passwords;
using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoardGameTools.Application.Users.Commands.AddUser
{
    public record AddUserCommand(string Email, string Password, string ConfirmPassword, string ConfirmationLinkBase) : IRequest<Guid>;

    public class AddUserCommandHandler(
        IAppDbContext context, 
        IPasswordHasher passwordHasher,
        IEmailSender emailSender,
        IEmailTemplate emailTemplate,
        ILogger<AddUserCommandHandler> logger
        ) : IRequestHandler<AddUserCommand, Guid>
    {
        private readonly IAppDbContext _context = context;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IEmailTemplate _emailTemplate = emailTemplate;
        private readonly ILogger<AddUserCommandHandler> _logger = logger;

        public async Task<Guid> Handle(AddUserCommand request, CancellationToken ct)
        {
            var email = Email.Create(request.Email);

            var exists = await _context.Users.AnyAsync(u => u.Email == email, ct);
            if (exists)
                throw new InvalidOperationException("Un compte existe déjà avec cet email.");

            var hashedPassword = _passwordHasher.Hash(request.Password);
            var user = new User(email, hashedPassword);

            var token = Guid.NewGuid().ToString();
            user.SetEmailConfirmationToken(token);

            _context.Users.Add(user);
            await _context.SaveChangesAsync(ct);

            try
            {
                var confirmationLink = $"{request.ConfirmationLinkBase}/confirm-email?token={Uri.EscapeDataString(token)}";
                string emailContent = _emailTemplate.BuildEmailConfirmation(confirmationLink);
                await _emailSender.SendAsync(email.Value, "Confirme ton compte", emailContent, ct);
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Echec lors de l'envoi du mail de confirmation de compte pour l'usager {UserId} ({Email})",
                    user.Id,
                    email.Value);
            }

            return user.Id;
        }
    }
}
