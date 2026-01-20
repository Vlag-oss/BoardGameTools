using BoardGameTools.Application.Services.Passwords;
using BoardGameTools.Application.Users.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BoardGameTools.FunctionalTests.Users.Commands
{
    public class AddUserCommandHandlerTests(PostgresContainer container) : DatabaseTestBase(container)
    {
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();

        [Fact]
        public async Task Handle_Shoud_AddUserAndReturnId()
        {
            //Arrange
            await using var context = CreateDbContext();
            var handler = new AddUserCommandHandler(context, _passwordHasherMock.Object);
            var command = new AddUserCommand("test@gmail.com", "Password123!", "Password123!");

            _passwordHasherMock.Setup(ph => ph.Hash(It.IsAny<string>())).Returns("hashedPassword");

            //Act
            var id = await handler.Handle(command, CancellationToken.None);

            //Assert
            var saved = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            saved.Should().NotBeNull();
            saved!.Email.Value.Should().Be(command.Email);
            saved!.PasswordHash.Should().Be("hashedPassword");
        }
    }
}
