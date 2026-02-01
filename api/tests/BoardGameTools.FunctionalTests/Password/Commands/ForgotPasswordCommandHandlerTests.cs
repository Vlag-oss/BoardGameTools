using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Common.Options;
using BoardGameTools.Application.Password.Commands;
using BoardGameTools.Application.Services.Tokens;
using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BoardGameTools.FunctionalTests.Password.Commands
{
    public class ForgotPasswordCommandHandlerTests : DatabaseTestBase
    {
        private readonly Mock<IEmailTemplate> _templateMock = new();
        private readonly Mock<IEmailSender> _mailSenderMock = new();
        private readonly ITokenService _tokenService;
        
        private readonly IOptions<ClientOptions> _clientOptions = Options.Create(new ClientOptions
        {
            BaseUrl = "https://boardgametools.test"
        });
        private readonly IOptions<TokenOptions> _tokenOptions = Options.Create(new TokenOptions
        {
            Audience = "test_audience",
            Issuer = "test_issuer",
            AccessTokenExpirationMinutes = 15,
            RefreshTokenExpirationDays = 1,
            SigningKey = "super_secret_signing_key_123456789_123"
        });
        private readonly Mock<IDateTimeProvider> _dateTimeMock = new();

        private readonly Mock<ILogger<ForgotPasswordCommandHandler>> _loggerMock = new();

        public ForgotPasswordCommandHandlerTests(PostgresContainer db) : base(db)
        {
            _tokenService = new TokenService(_tokenOptions, _dateTimeMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturn_WhenEmailNotFound()
        {
            //Arrange
            var user = User.Create(Email.Create("user1@gmail.com"), "hashed-password");
            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new ForgotPasswordCommandHandler(context, _templateMock.Object, _mailSenderMock.Object, _tokenService, _clientOptions, _loggerMock.Object);
            var command = new ForgotPasswordCommand("test@gmail.com");

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldReturn_WhenEmailFoundButNotConfirmed()
        {
            //Arrange
            var user = User.Create(Email.Create("user1@gmail.com"), "hashed-password");
            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new ForgotPasswordCommandHandler(context, _templateMock.Object, _mailSenderMock.Object, _tokenService, _clientOptions, _loggerMock.Object);
            var command = new ForgotPasswordCommand("user1@gmail.com");

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldAddResetPasswordToken_WhenEmailExistAndConfirmed()
        {
            //Arrange
            var user = User.Create(Email.Create("user1@gmail.com"), "hashed-password");
            user.ConfirmEmail();
            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new ForgotPasswordCommandHandler(context, _templateMock.Object, _mailSenderMock.Object, _tokenService, _clientOptions, _loggerMock.Object);
            var command = new ForgotPasswordCommand("user1@gmail.com");

            _templateMock.Setup(t => t.BuildEmailForgotPassword(It.IsAny<string>())).Returns("<html></html>");

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            var passwordResetToken = context.PasswordResetToken.AsNoTracking().FirstOrDefault(r => r.UserId == user.Id);
            passwordResetToken.Should().NotBeNull();
            passwordResetToken.TokenHash.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_ShouldSendMail_WhenEmailExistAndConfirmed()
        {
            //Arrange
            var user = User.Create(Email.Create("user1@gmail.com"), "hashed-password");
            user.ConfirmEmail();
            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new ForgotPasswordCommandHandler(context, _templateMock.Object, _mailSenderMock.Object, _tokenService, _clientOptions, _loggerMock.Object);
            var command = new ForgotPasswordCommand("user1@gmail.com");

            var emailContent = "<html></html>";
            _templateMock.Setup(t => t.BuildEmailForgotPassword(It.IsAny<string>())).Returns(emailContent);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            _templateMock.Verify(t => t.BuildEmailForgotPassword(It.IsAny<string>()), Times.Once);
            _mailSenderMock.Verify(s => s.SendAsync(command.Email, "Réinitialisation du mot de passe", emailContent, CancellationToken.None), Times.Once);
            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldLog_WhenEmailThrowException()
        {
            //Arrange
            var user = User.Create(Email.Create("user1@gmail.com"), "hashed-password");
            user.ConfirmEmail();
            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new ForgotPasswordCommandHandler(context, _templateMock.Object, _mailSenderMock.Object, _tokenService, _clientOptions, _loggerMock.Object);
            var command = new ForgotPasswordCommand("user1@gmail.com");

            var emailContent = "<html></html>";
            _templateMock.Setup(t => t.BuildEmailForgotPassword(It.IsAny<string>())).Returns(emailContent);

            var ex = new Exception("Ceci est un test");
            _mailSenderMock.Setup(m => m.SendAsync("user1@gmail.com", "Réinitialisation du mot de passe", emailContent, It.IsAny<CancellationToken>())).ThrowsAsync(ex);

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            _loggerMock.Verify(x => x.Log(
                 LogLevel.Error,
                 It.IsAny<EventId>(),
                 It.Is<It.IsAnyType>((state, _) =>
                     state.ToString()!.Contains("Echec lors de l'envoi du mail de réinitialisation du mot de passe")),
                 ex,
                 It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                 Times.Once);
        }
    }
}
