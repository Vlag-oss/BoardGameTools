using BoardGameTools.Application.LibraryGames.Queries.GetLibraryGames;
using BoardGameTools.Domain.Entities;
using FluentAssertions;

namespace BoardGameTools.FunctionalTests.LibraryGames.Queries
{
    public class GetLibraryGamesQueryHandlerTests(PostgresContainer container) : DatabaseTestBase(container)
    {
        [Fact]
        public async Task Handle_Should_ListOfLibraryGame()
        {
            //Arrange
            var ownerId = Guid.NewGuid();

            await AddRangeAsync(
            [
                LibraryGame.CreateFromBgg(ownerId, "Carcassonne", 10),
                LibraryGame.CreateFromBgg(ownerId, "Catan", 11),
                LibraryGame.CreateManual(Guid.NewGuid(), "Terraforming Mars")
            ]);

            await using var context = CreateDbContext();
            var handler = new GetLibraryGamesQueryHandler(context);

            var query = new GetLibraryGamesQuery(ownerId);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().HaveCount(2);
            result.Select(x => x.Name).Should().BeEquivalentTo(["Carcassonne", "Catan"]);
        }
    }
}
