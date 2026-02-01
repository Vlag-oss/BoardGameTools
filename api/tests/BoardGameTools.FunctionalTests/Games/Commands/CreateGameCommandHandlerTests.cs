using BoardGameTools.Application.Library.Games.Commands;
using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.FunctionalTests.Games.Commands
{
    public class CreateGameCommandHandlerTests(PostgresContainer container) : DatabaseTestBase(container)
    {
        [Fact]
        public async Task Handle_Should_AddManualGameAndReturnId()
        {
            //Arrange
            var user = User.Create(Email.Create("test@test.com"), "hashed-password");
            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new CreateGameCommandHandler(context);
            var command = new CreateGameCommand(user.Id, "Test Game", "Manual", null);

            //Act
            var id = await handler.Handle(command, CancellationToken.None);

            //Assert
            var saved = await context.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);

            saved.Should().NotBeNull();
            saved!.OwnerId.Should().Be(command.OwnerId);
            saved.Name.Should().Be("Test Game");
            saved.Source.Should().Be("Manual");
            saved.SourceGameId.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_AddBggGameAndReturnId()
        {
            //Arrange
            var user = User.Create(Email.Create("test@test.com"), "hashed-password");
            await AddAsync(user);

            await using var context = CreateDbContext();
            var handler = new CreateGameCommandHandler(context);
            var command = new CreateGameCommand(user.Id, "Catan", "BGG", "13");

            //Act
            var id = await handler.Handle(command, CancellationToken.None);

            //Assert
            var saved = await context.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);

            saved.Should().NotBeNull();
            saved!.OwnerId.Should().Be(command.OwnerId);
            saved.Name.Should().Be("Catan");
            saved.Source.Should().Be("BGG");
            saved.SourceGameId.Should().Be("13");
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenGameAlreadyExists()
        {
            //Arrange
            var user = User.Create(Email.Create("test@test.com"), "hashed-password");
            await AddAsync(user);
            await AddAsync(Game.CreateManual(user.Id, "Carcassonne"));

            var handler = new CreateGameCommandHandler(CreateDbContext());
            var command = new CreateGameCommand(user.Id, "Carcassonne", "Manual", null);

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Le jeu existe déjà dans votre library");
        }
    }
}
