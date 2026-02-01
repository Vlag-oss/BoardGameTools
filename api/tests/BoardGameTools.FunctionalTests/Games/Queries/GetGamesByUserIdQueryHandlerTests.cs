using BoardGameTools.Application.Library.Games.Queries;
using BoardGameTools.Domain.Entities;
using BoardGameTools.Domain.ValueObjects;
using FluentAssertions;

namespace BoardGameTools.FunctionalTests.Games.Queries
{
    public class GetGamesByUserIdQueryHandlerTests(PostgresContainer container) : DatabaseTestBase(container)
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
                Game.CreateFromBgg(user1.Id, "Carcassonne", 10),
                Game.CreateFromBgg(user1.Id, "Catan", 11),
                Game.CreateManual(user2.Id, "Terraforming Mars")
            ]);

            await using var context = CreateDbContext();
            var handler = new GetGamesByUserIdQueryHandler(context);

            var query = new GetGamesByUserIdQuery(user1.Id);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().HaveCount(2);
            result.Select(x => x.Name).Should().BeEquivalentTo(["Carcassonne", "Catan"]);
        }
    }
}
