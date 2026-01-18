using BoardGameTools.Application.LibraryGames.Commands.AddLibraryGame;
using BoardGameTools.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.FunctionalTests.LibraryGames.Commands
{
    public class AddLibraryGameCommandHandlerTests(PostgresContainer container) : DatabaseTestBase(container)
    {
        [Fact]
        public async Task Handle_Should_AddManualGameAndReturnId()
        {
            //Arrange
            await using var context = CreateDbContext();
            var handler = new AddLibraryGameCommandHandler(context);
            var command = new AddLibraryGameCommand(Guid.NewGuid(), "Test Game", "Manual", null);

            //Act
            var id = await handler.Handle(command, CancellationToken.None);

            //Assert
            var saved = await context.LibraryGames.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);

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
            await using var context = CreateDbContext();
            var handler = new AddLibraryGameCommandHandler(context);
            var command = new AddLibraryGameCommand(Guid.NewGuid(), "Catan", "BGG", "13");

            //Act
            var id = await handler.Handle(command, CancellationToken.None);

            //Assert
            var saved = await context.LibraryGames.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);

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
            var ownerId = Guid.NewGuid();

            await AddAsync(LibraryGame.CreateManual(ownerId, "Carcassonne"));

            var handler = new AddLibraryGameCommandHandler(CreateDbContext());
            var command = new AddLibraryGameCommand(ownerId, "Carcassonne", "Manual", null);

            //Act
            Func<Task> func = () => handler.Handle(command, CancellationToken.None);

            //Assert
            await func.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Le jeu existe déjà dans votre library");
        }
    }
}
