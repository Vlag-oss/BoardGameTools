using BoardGameTools.Domain.Entities;
using FluentAssertions;

namespace BoardGameTools.UnitTests
{
    public class LibraryGameTests
    {
        [Fact]
        public void CreateManual_Should_SetNameAndSourceAndOwnerId()
        {
            //Arrange
            var ownerId = Guid.NewGuid();

            //Act
            var game = LibraryGame.CreateManual(ownerId, "Harmonies");

            //Assert
            game.OwnerId.Should().Be(ownerId);
            game.Name.Should().Be("Harmonies");
            game.Source.Should().Be("Manual");
            game.SourceGameId.Should().BeNull();
        }

        [Fact]
        public void CreateFromBgg_Should_SetNameAndSourceAndGameIdAndOwnerId()
        {
            //Arrange
            var ownerId = Guid.NewGuid();

            //Act
            var game = LibraryGame.CreateFromBgg(ownerId, "Harmonies", 123);

            //Assert
            game.OwnerId.Should().Be(ownerId);
            game.Name.Should().Be("Harmonies");
            game.Source.Should().Be("BGG");
            game.SourceGameId.Should().Be("123");
        }

        [Fact]
        public void CreateManual_Should_Trim_Name()
        {
            //Arrange
            var ownerId = Guid.NewGuid();

            //Act
            var game = LibraryGame.CreateManual(ownerId, "   Harmonies  ");

            //Assert
            game.Name.Should().Be("Harmonies");
        }

        [Fact]
        public void CreateFromBgg_Should_Trim_Name()
        {
            //Arrange
            var ownerId = Guid.NewGuid();

            //Act
            var game = LibraryGame.CreateFromBgg(ownerId, "   Harmonies  ", 123);

            //Assert
            game.Name.Should().Be("Harmonies");
        }

        [Fact]
        public void CreateManual_Should_Throw_WhenNameIsEmpty()
        {
            //Arrange
            var ownerId = Guid.NewGuid();

            //Act
            Action act = () => LibraryGame.CreateManual(ownerId, string.Empty);

            //Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Le nom du jeu de société est obligatoire.*")
                .And.ParamName.Should().Be("name");
        }

        [Fact]
        public void CreateFromBgg_Should_Throw_WhenNameIsEmpty()
        {
            //Arrange
            var ownerId = Guid.NewGuid();

            //Act
            Action act = () => LibraryGame.CreateFromBgg(ownerId, string.Empty, 123);

            //Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Le nom du jeu de société est obligatoire.*")
                .And.ParamName.Should().Be("name");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-42)]
        public void CreateFromBgg_Should_Throw_WhenBggIdIsNotPositiveOrEqualZero(int invalidBggId)
        {
            //Arrange
            var ownerId = Guid.NewGuid();

            //Act
            Action act = () => LibraryGame.CreateFromBgg(ownerId, "Harmonies", invalidBggId);

            //Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("L'identifiant BGG doit être un entier positif. (Parameter 'bggId')")
                .And.ParamName.Should().Be("bggId");
        }
    }
}
