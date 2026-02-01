using BoardGameTools.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.Application.Auth.ConfirmEmail.Commands
{
    public record ConfirmEmailCommand(string Token) : IRequest;

    public class ConfirmEmailCommandHandler(IAppDbContext context) : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly IAppDbContext _context = context;

        public async Task Handle(ConfirmEmailCommand request, CancellationToken ct)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.EmailConfirmationToken == request.Token, ct) ?? throw new InvalidOperationException("Le lien de configuration est invalide");
            user.ConfirmEmail();
            await _context.SaveChangesAsync(ct);
        }
    }
}
