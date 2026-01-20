using BoardGameTools.Application.LibraryGames.Commands.AddLibraryGame;
using FluentAssertions;

namespace BoardGameTools.UnitTests.LibraryGames
{
    public class AddLibraryGameCommandValidatorTests
    {
        [Fact]
        public void Should_Fail_WhenOwnerIsEmpty()
        {
            var validator = new AddLibraryGameCommandValidator();

            var result = validator.Validate(new AddLibraryGameCommand(Guid.Empty, "Catan", "Manual", null));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "OwnerId");
        }

        [Fact]
        public void Should_Fail_WhenNameIsEmpty()
        {
            var validator = new AddLibraryGameCommandValidator();

            var result = validator.Validate(new AddLibraryGameCommand(Guid.NewGuid(), string.Empty, "Manual", null));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Should_Fail_WhenSourceIsEmpty()
        {
            var validator = new AddLibraryGameCommandValidator();

            var result = validator.Validate(new AddLibraryGameCommand(Guid.NewGuid(), "Catan", string.Empty, null));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Source");
        }

        [Fact]
        public void Should_Fail_WhenSourceIsNotManualOrBgg()
        {
            var validator = new AddLibraryGameCommandValidator();

            var result = validator.Validate(new AddLibraryGameCommand(Guid.NewGuid(), "Catan", "Test", null));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "La source doit être 'manual' ou 'bgg'");
        }

        [Fact]
        public void Should_Fail_WhenSourceIsBggAndSourceGameIdIsNull()
        {
            var validator = new AddLibraryGameCommandValidator();

            var result = validator.Validate(new AddLibraryGameCommand(Guid.NewGuid(), "Catan", "bgg", null));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "SourceGameId doit être un id BGG valide");
        }
    }
}
