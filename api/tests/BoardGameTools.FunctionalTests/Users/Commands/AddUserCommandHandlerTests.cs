using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Services.Passwords;
using BoardGameTools.Application.Users.Commands.AddUser;
using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BoardGameTools.FunctionalTests.Users.Commands
{
    public class AddUserCommandHandlerTests(PostgresContainer container) : DatabaseTestBase(container)
    {
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<IEmailSender> _mailSenderMock = new();
        private readonly Mock<IEmailTemplate> _templateMock = new();
        private readonly Mock<ILogger<AddUserCommandHandler>> _loggerMock = new();

        [Fact]
        public async Task Handle_Shoud_AddUserAndReturnId()
        {
            //Arrange
            await using var context = CreateDbContext();
            var handler = new AddUserCommandHandler(context, _passwordHasherMock.Object, _mailSenderMock.Object, _templateMock.Object, _loggerMock.Object);
            var command = new AddUserCommand("test@gmail.com", "Password123!", "Password123!", string.Empty);

            _passwordHasherMock.Setup(ph => ph.Hash(It.IsAny<string>())).Returns("hashedPassword");
            var emailContent = "<html></html>";
            _templateMock.Setup(t => t.BuildEmailConfirmation(It.IsAny<string>())).Returns(emailContent);

            //Act
            var id = await handler.Handle(command, CancellationToken.None);

            //Assert
            var saved = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            saved.Should().NotBeNull();
            saved!.Email.Value.Should().Be(command.Email);
            saved!.PasswordHash.Should().Be("hashedPassword");

            _templateMock.Verify(t => t.BuildEmailConfirmation(It.IsAny<string>()), Times.Once);
            _mailSenderMock.Verify(m => m.SendAsync("test@gmail.com", "Confirme ton compte", emailContent, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenUserWithSameEmailExists()
        {
            //Arrange
            var user = User.Create(Email.Create("test@test.com"), "hashed-password");
            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new AddUserCommandHandler(context, _passwordHasherMock.Object, _mailSenderMock.Object, _templateMock.Object, _loggerMock.Object);
            var command = new AddUserCommand("test@test.com", "Password123!", "Password123!", string.Empty);

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<InvalidOperationException>().WithMessage("Un compte existe déjà avec cet email.");
        }

        [Fact]
        public async Task Handle_ShouldLog_WhenEmailThrowException()
        {
            //Arrange
            await using var context = CreateDbContext();
            _passwordHasherMock.Setup(ph => ph.Hash(It.IsAny<string>())).Returns("hashedPassword");

            var emailContent = "<html></html>";
            _templateMock.Setup(t => t.BuildEmailConfirmation(It.IsAny<string>())).Returns(emailContent);

            var ex = new Exception("Ceci est un test");
            _mailSenderMock.Setup(m => m.SendAsync("test@gmail.com", "Confirme ton compte", emailContent, It.IsAny<CancellationToken>())).ThrowsAsync(ex);

            var handler = new AddUserCommandHandler(context, _passwordHasherMock.Object, _mailSenderMock.Object, _templateMock.Object, _loggerMock.Object);
            var command = new AddUserCommand("test@gmail.com", "Password123!", "Password123!", string.Empty);

            //Act
            var userId = await handler.Handle(command, CancellationToken.None);

            //Assert
            _loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) =>
                    state.ToString()!.Contains("Echec lors de l'envoi du mail de confirmation")),
                ex,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
