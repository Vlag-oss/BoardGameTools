using BoardGameTools.Application.Auth.ResetPassword;
using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Services.Passwords;
using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BoardGameTools.FunctionalTests.ResetPassword
{
    public class ResetPasswordCommandHandlerTests(PostgresContainer db) : DatabaseTestBase(db)
    {
        private readonly Mock<IDateTimeProvider> _dateTimeMock = new();
        private readonly Mock<IPasswordHasher> _hasherMock = new();
        private readonly Mock<ILogger<ResetPasswordCommandHandler>> _loggerMock = new();

        [Fact]
        public async Task Handle_ShouldThrow_WhenResetTokenNotFound()
        {
            //Arrange
            var user = User.Create(Email.Create("test@gmail.com"), "hashed_password");
            user.ConfirmEmail();
            await AddAsync(user);

            var resetPassword = PasswordResetToken.Create("123456789", DateTime.UtcNow.AddHours(1), user.Id);
            await AddAsync(resetPassword);

            await using var context = CreateDbContext();
            var handler = new ResetPasswordCommandHandler(context, _dateTimeMock.Object, _hasherMock.Object, _loggerMock.Object);

            var command = new ResetPasswordCommand("invalid_token", "Password123!", "Password123!");

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<ResetPasswordTokenException>().WithMessage("Le jeton de réinitialisation est invalide.");
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenResetTokenIsUsed()
        {
            //Arrange
            var user = User.Create(Email.Create("test@gmail.com"), "hashed_password");
            user.ConfirmEmail();
            await AddAsync(user);

            var tokenHash = "123456789";
            var resetPassword = PasswordResetToken.Create(tokenHash, DateTime.UtcNow.AddHours(1), user.Id);
            resetPassword.MarkAsUsed();
            await AddAsync(resetPassword);

            await using var context = CreateDbContext();
            var handler = new ResetPasswordCommandHandler(context, _dateTimeMock.Object, _hasherMock.Object, _loggerMock.Object);

            var command = new ResetPasswordCommand(tokenHash, "Password123!", "Password123!");

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<ResetPasswordTokenException>().WithMessage("Le jeton de réinitialisation est invalide.");
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenResetTokenIsExpired()
        {
            //Arrange
            var user = User.Create(Email.Create("test@gmail.com"), "hashed_password");
            user.ConfirmEmail();
            await AddAsync(user);

            var expires = new DateTime(2026, 02, 01, 10, 50, 00, DateTimeKind.Utc);
            var tokenHash = "123456789";
            var resetPassword = PasswordResetToken.Create(tokenHash, expires, user.Id);
            await AddAsync(resetPassword);

            await using var context = CreateDbContext();
            _dateTimeMock.Setup(d => d.UtcNow).Returns(new DateTime(2026, 02, 01, 10, 55, 00));
            var handler = new ResetPasswordCommandHandler(context, _dateTimeMock.Object, _hasherMock.Object, _loggerMock.Object);

            var command = new ResetPasswordCommand(tokenHash, "Password123!", "Password123!");

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<ResetPasswordTokenException>().WithMessage("Le jeton de réinitialisation est invalide.");
        }

        [Fact]
        public async Task Handle_ShouldUpdatePassword_WhenTokenIsValid()
        {
            //Arrange
            var user = User.Create(Email.Create("test@gmail.com"), "hashed_password");
            user.ConfirmEmail();
            await AddAsync(user);

            var expires = new DateTime(2026, 02, 01, 10, 50, 00, DateTimeKind.Utc);
            var tokenHash = "123456789";
            var resetPassword = PasswordResetToken.Create(tokenHash, expires, user.Id);
            await AddAsync(resetPassword);

            await using var context = CreateDbContext();
            _dateTimeMock.Setup(d => d.UtcNow).Returns(new DateTime(2026, 02, 01, 10, 45, 00));
            var handler = new ResetPasswordCommandHandler(context, _dateTimeMock.Object, _hasherMock.Object, _loggerMock.Object);

            var newPassword = "Password123!";
            var command = new ResetPasswordCommand(tokenHash, newPassword, "Password123!");

            _hasherMock.Setup(h => h.Hash(newPassword)).Returns("hashed-Password123!");
            //Act
            await handler.Handle(command, CancellationToken.None);

            //Assert
            var savedUser = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == user.Id);
            savedUser.Should().NotBeNull();
            savedUser.PasswordHash.Should().Be("hashed-Password123!");

            var savedResetToken = await context.PasswordResetToken.AsNoTracking().FirstOrDefaultAsync(p => p.Id == resetPassword.Id);
            savedResetToken.Should().NotBeNull();
            savedResetToken.IsUsed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenNewPasswordIsSameAsOldPassword()
        {
            //Arrange
            var passwordHasher = new BCryptPasswordHasher();
            var hashedPassword = passwordHasher.Hash("Password123!");

            var user = User.Create(Email.Create("test@gmail.com"), hashedPassword);
            user.ConfirmEmail();
            await AddAsync(user);

            var expires = new DateTime(2026, 02, 01, 10, 50, 00, DateTimeKind.Utc);
            var tokenHash = "123456789";
            var resetPassword = PasswordResetToken.Create(tokenHash, expires, user.Id);
            await AddAsync(resetPassword);

            await using var context = CreateDbContext();
            _dateTimeMock.Setup(d => d.UtcNow).Returns(new DateTime(2026, 02, 01, 10, 45, 00));
            var handler = new ResetPasswordCommandHandler(context, _dateTimeMock.Object, passwordHasher, _loggerMock.Object);

            var newPassword = "Password123!";
            var command = new ResetPasswordCommand(tokenHash, newPassword, "Password123!");

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<SamePasswordException>().WithMessage("Le nouveau mot de passe ne peut pas être le même que l'ancien.");
        }
    }
}
