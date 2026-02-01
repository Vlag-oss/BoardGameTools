using BoardGameTools.Application.Auth.Login.Commands;
using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Common.Options;
using BoardGameTools.Application.Services.Passwords;
using BoardGameTools.Application.Services.Tokens;
using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace BoardGameTools.FunctionalTests.Login
{
    public class LoginCommandHanderTests : DatabaseTestBase
    {
        private readonly Mock<IDateTimeProvider> _dateTimeMock = new();
        private readonly BCryptPasswordHasher _passwordHasher = new();

        private readonly IOptions<AuthOptions> _options = Options.Create(new AuthOptions { MaxFailed = 1, AccountLockDuration = TimeSpan.FromMinutes(10) });
        private readonly IOptions<TokenOptions> _tokenOptions = Options.Create(new TokenOptions
        {
            Audience = "test_audience",
            Issuer = "test_issuer",
            AccessTokenExpirationMinutes = 15,
            RefreshTokenExpirationDays = 1,
            SigningKey = "super_secret_signing_key_123456789_123"
        });

        private readonly TokenService _tokenService;

        public LoginCommandHanderTests(PostgresContainer db) : base(db)
        {
            _tokenService = new TokenService(_tokenOptions, _dateTimeMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenUserDoesntExist()
        {
            // Arrange
            var user = User.Create(Email.Create("test@gmail.com"), "hashed-password");
            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new LoginCommandHandler(context, _dateTimeMock.Object, _passwordHasher, _tokenService, _options);

            var command = new LoginCommand("doesntExist@gmail.com", "Password123!");

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<InvalidCredentialsException>().WithMessage("Email ou mot de passe incorrect.");
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenEmailIsNotConfirmed()
        {
            //Arrange
            var user = User.Create(Email.Create("test@gmail.com"), "hashed-password");
            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new LoginCommandHandler(context, _dateTimeMock.Object, _passwordHasher, _tokenService, _options);

            var command = new LoginCommand("test@gmail.com", "Password123!");

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<EmailNotConfirmedException>().WithMessage("L'email n'a pas encore été confirmé.");
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenUserIsLocked()
        {
            //Arrange
            var user = User.Create(Email.Create("test@gmail.com"), "hashed-password");
            user.ConfirmEmail();
            user.Lock();

            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new LoginCommandHandler(context, _dateTimeMock.Object, _passwordHasher, _tokenService, _options);

            var command = new LoginCommand("test@gmail.com", "Password123!");

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<AccountLockedException>().WithMessage("Votre compte est verrouillé, réessayez plus tard.");
        }

        [Fact]
        public async Task Handle_ShouldUnlock_WhenUserIsLockedAndTimeHasElapsed()
        {
            //Arrange
            var now = new DateTime(2026, 01, 01, 12, 00, 00, DateTimeKind.Utc);
            _dateTimeMock.Setup(d => d.UtcNow).Returns(now);
            var hashedPassword = _passwordHasher.Hash("Password123!");

            var user = User.Create(Email.Create("test@gmail.com"), hashedPassword);
            user.ConfirmEmail();
            user.Lock();
            user.RegisterFailedLoginAttempt(now.AddMinutes(-16));

            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new LoginCommandHandler(context, _dateTimeMock.Object, _passwordHasher, _tokenService, _options);

            var command = new LoginCommand("test@gmail.com", "Password123!");

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            var saved = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == user.Email);

            saved.Should().NotBeNull();
            saved!.IsLocked.Should().BeFalse();
            saved.FailedLoginAttempts.Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldSetLockUser_WhenMaxFailedAttemptsExceeded()
        {
            //Arrange
            var now = new DateTime(2026, 01, 01, 12, 00, 00, DateTimeKind.Utc);
            _dateTimeMock.Setup(d => d.UtcNow).Returns(now);
            var hashedPassword = _passwordHasher.Hash("Password123!");

            var user = User.Create(Email.Create("test@gmail.com"), hashedPassword);
            user.ConfirmEmail();
            user.RegisterFailedLoginAttempt(now.AddMinutes(-2));

            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new LoginCommandHandler(context, _dateTimeMock.Object, _passwordHasher, _tokenService, _options);

            var command = new LoginCommand("test@gmail.com", "Password123456!");

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<InvalidCredentialsException>().WithMessage("Email ou mot de passe incorrect.");

            var saved = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == user.Email);

            saved.Should().NotBeNull();
            saved!.IsLocked.Should().BeTrue();
            saved.FailedLoginAttempts.Should().Be(2);
            saved.LastFailedLogin.Should().Be(now);
        }

        [Fact]
        public async Task Handle_ShoulUnlockAndIncrementFailedLogin_WhenTimeHasElapsedAndPasswordIsIncorrect()
        {
            //Arrange
            var now = new DateTime(2026, 01, 01, 12, 0, 0, DateTimeKind.Utc);
            _dateTimeMock.Setup(d => d.UtcNow).Returns(now);

            var hashedPassword = _passwordHasher.Hash("Password123!");

            var user = User.Create(Email.Create("test@gmail.com"), hashedPassword);
            user.ConfirmEmail();
            user.RegisterFailedLoginAttempt(now.AddMinutes(-16));
            user.Lock();

            await AddAsync(user);

            var options = Options.Create(new AuthOptions { MaxFailed = 2, AccountLockDuration = TimeSpan.FromMinutes(10) });

            await using var context = CreateDbContext();
            var handler = new LoginCommandHandler(context, _dateTimeMock.Object, _passwordHasher, _tokenService, options);

            var command = new LoginCommand("test@gmail.com", "Password123456!");

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<InvalidCredentialsException>().WithMessage("Email ou mot de passe incorrect.");

            var saved = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == user.Email);

            saved.Should().NotBeNull();
            saved!.IsLocked.Should().BeFalse();
            saved.FailedLoginAttempts.Should().Be(1);
            saved.LastFailedLogin.Should().Be(now);
        }

        [Fact]
        public async Task Handle_ShouldReturnTokens_WhenCredentialsAreValid()
        {
            //Arrange
            var now = new DateTime(2026, 01, 01, 12, 0, 0, DateTimeKind.Utc);
            _dateTimeMock.Setup(d => d.UtcNow).Returns(now);
            
            var hashedPassword = _passwordHasher.Hash("Password123!");
            var user = User.Create(Email.Create("test@gmail.com"), hashedPassword);
            user.ConfirmEmail();

            await AddAsync(user);
            var options = Options.Create(new AuthOptions { MaxFailed = 2, AccountLockDuration = TimeSpan.FromMinutes(10) });

            await using var context = CreateDbContext();
            var handler = new LoginCommandHandler(context, _dateTimeMock.Object, _passwordHasher, _tokenService, options);

            var command = new LoginCommand("test@gmail.com", "Password123!");

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result?.AccessToken.Should().NotBeNullOrEmpty();
            result?.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_ShouldAddRefreshTokens_WhenCredentialsAreValid()
        {
            //Arrange
            var now = new DateTime(2026, 01, 01, 12, 0, 0, DateTimeKind.Utc);
            _dateTimeMock.Setup(d => d.UtcNow).Returns(now);

            var hashedPassword = _passwordHasher.Hash("Password123!");
            var user = User.Create(Email.Create("test@gmail.com"), hashedPassword);
            user.ConfirmEmail();

            await AddAsync(user);
            var options = Options.Create(new AuthOptions { MaxFailed = 2, AccountLockDuration = TimeSpan.FromMinutes(10) });

            await using var context = CreateDbContext();
            var handler = new LoginCommandHandler(context, _dateTimeMock.Object, _passwordHasher, _tokenService, options);

            var command = new LoginCommand("test@gmail.com", "Password123!");

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            var refreshToken = await context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(rt => rt.UserId == user.Id);
            refreshToken.Should().NotBeNull();
            refreshToken.TokenHash.Should().NotBeNull();
        }
    }
}
