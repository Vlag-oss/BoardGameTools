using BoardGameTools.Application.Auth.ConfirmEmail.Commands;
using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.FunctionalTests.ConfirmEmail
{
    public class ConfirmEmailCommandHandlerTests(PostgresContainer container) : DatabaseTestBase(container)
    {
        [Fact]
        public async Task Handle_ShouldConfirmEmail_WhenTokenIsValid()
        {
            //Arrange
            var token = "123-456-789";

            var user = User.Create(Email.Create("test@test.com"), "hashed-password");
            user.RequireEmailConfirmation(token);
            await AddAsync(user);

            await using var context = CreateDbContext();
            var command = new ConfirmEmailCommand(token);
            var handler = new ConfirmEmailCommandHandler(context);

            //Act
            await handler.Handle(command, CancellationToken.None);

            //Assert
            await using var assertContext = CreateDbContext();
            var savedUser = await assertContext.Users
                .AsNoTracking()
                .SingleAsync(u => u.Id == user.Id);

            savedUser.EmailConfirmed.Should().BeTrue();
            savedUser.EmailConfirmationToken.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenTokenIsInvalid()
        {
            //Arrange
            var token = "123-456-789";
            var user = User.Create(Email.Create("test@test.com"), "hashed-password");
            user.RequireEmailConfirmation(token);
            await AddAsync(user);

            await using var context = CreateDbContext();
            var command = new ConfirmEmailCommand("234-567-891");
            var handler = new ConfirmEmailCommandHandler(context);

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<InvalidOperationException>().WithMessage("Le lien de configuration est invalide");
        }
    }
}
