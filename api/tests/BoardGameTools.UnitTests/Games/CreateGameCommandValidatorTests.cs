using BoardGameTools.Application.Library.Games.Commands;
using FluentAssertions;

namespace BoardGameTools.UnitTests.Games
{
    public class CreateGameCommandValidatorTests
    {
        [Fact]
        public void Should_Fail_WhenOwnerIsEmpty()
        {
            var validator = new CreateGameCommandValidator();

            var result = validator.Validate(new CreateGameCommand(Guid.Empty, "Catan", "Manual", null));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "OwnerId");
        }

        [Fact]
        public void Should_Fail_WhenNameIsEmpty()
        {
            var validator = new CreateGameCommandValidator();

            var result = validator.Validate(new CreateGameCommand(Guid.NewGuid(), string.Empty, "Manual", null));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Should_Fail_WhenSourceIsEmpty()
        {
            var validator = new CreateGameCommandValidator();

            var result = validator.Validate(new CreateGameCommand(Guid.NewGuid(), "Catan", string.Empty, null));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Source");
        }

        [Fact]
        public void Should_Fail_WhenSourceIsNotManualOrBgg()
        {
            var validator = new CreateGameCommandValidator();

            var result = validator.Validate(new CreateGameCommand(Guid.NewGuid(), "Catan", "Test", null));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "La source doit être 'manual' ou 'bgg'");
        }

        [Fact]
        public void Should_Fail_WhenSourceIsBggAndSourceGameIdIsNull()
        {
            var validator = new CreateGameCommandValidator();

            var result = validator.Validate(new CreateGameCommand(Guid.NewGuid(), "Catan", "bgg", null));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "SourceGameId doit être un id BGG valide");
        }
    }
}
