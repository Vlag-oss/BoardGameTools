using BoardGameTools.Application.LibraryGames.Queries.GetLibraryGames;
using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using FluentAssertions;

namespace BoardGameTools.FunctionalTests.LibraryGames.Queries
{
    public class GetLibraryGamesQueryHandlerTests(PostgresContainer container) : DatabaseTestBase(container)
    {
        [Fact]
        public async Task Handle_Should_ListOfLibraryGame()
        {
            //Arrange
            var user1 = User.Create(Email.Create("test@test.com"), "hashed-password");
            var user2 = User.Create(Email.Create("test2@test.com"), "hashed-password");
            await AddAsync(user1);
            await AddAsync(user2);

            await AddRangeAsync(
            [
                LibraryGame.CreateFromBgg(user1.Id, "Carcassonne", 10),
                LibraryGame.CreateFromBgg(user1.Id, "Catan", 11),
                LibraryGame.CreateManual(user2.Id, "Terraforming Mars")
            ]);

            await using var context = CreateDbContext();
            var handler = new GetLibraryGamesQueryHandler(context);

            var query = new GetLibraryGamesQuery(user1.Id);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().HaveCount(2);
            result.Select(x => x.Name).Should().BeEquivalentTo(["Carcassonne", "Catan"]);
        }
    }
}
