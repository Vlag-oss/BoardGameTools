using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Common.Options;
using BoardGameTools.Application.Login.DTOs;
using BoardGameTools.Application.Services.Passwords;
using BoardGameTools.Application.Services.Tokens;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BoardGameTools.Application.Login.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

    public class LoginCommandHandler(
        IAppDbContext context, 
        IDateTimeProvider dateTimeProvider, 
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IOptions<AuthOptions> authOptions
        ) : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IAppDbContext _context = context;
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ITokenService _tokenService = tokenService;
        private readonly AuthOptions _options = authOptions.Value;

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, ct) ?? throw new InvalidCredentialsException();

            if (!user.EmailConfirmed)
                throw new EmailNotConfirmedException();

            if (user.IsLocked)
            {
                if(user.LastFailedLogin != null && user.LastFailedLogin.Value.Add(_options.AccountLockDuration) <= _dateTimeProvider.UtcNow)
                {
                    user.ResetFailedLoginAttempts();
                    user.Unlock();
                }
                else
                    throw new AccountLockedException();
            }

            if(!_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                user.RegisterFailedLoginAttempt(_dateTimeProvider.UtcNow);

                if (user.FailedLoginAttempts >= _options.MaxFailed)
                    user.Lock();

                await _context.SaveChangesAsync(ct);
                throw new InvalidCredentialsException();
            }

            user.ResetFailedLoginAttempts();

            var accessToken = _tokenService.CreateAccessToken(user);
            var refreshToken = _tokenService.CreateRefreshToken(user);

            _context.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync(ct);
            return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken.TokenHash };
        }
    }
}
