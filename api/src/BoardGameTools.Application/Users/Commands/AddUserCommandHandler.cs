using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Services.Passwords;
using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using MediatR;

namespace BoardGameTools.Application.Users.Commands
{
    public record AddUserCommand(string Email, string Password, string ConfirmPassword) : IRequest<Guid>;

    public class AddUserCommandHandler(IAppDbContext context, IPasswordHasher passwordHasher) : IRequestHandler<AddUserCommand, Guid>
    {
        private readonly IAppDbContext _context = context;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<Guid> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var hashedPassword = _passwordHasher.Hash(request.Password);
            var email = Email.Create(request.Email);
            var user = new User(email, hashedPassword);

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
