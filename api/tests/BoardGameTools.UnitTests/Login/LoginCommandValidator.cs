using BoardGameTools.Application.Login.Commands;
using FluentAssertions;

namespace BoardGameTools.UnitTests.Login
{
    public class LoginCommandValidatorTests
    {
        [Fact]
        public void Should_Fail_WhenEmailIsEmpty()
        {
            //Arrange
            var validator = new LoginCommandValidator();

            //Act
            var result = validator.Validate(new LoginCommand(string.Empty, "Password123!"));

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage == "L'email est requis.");
        }

        [Fact]
        public void Should_Fail_WhenEmailIsInvalid()
        {
            //Arrange
            var validator = new LoginCommandValidator();
            //Act
            var result = validator.Validate(new LoginCommand("invalid-email", "Password123!"));
            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage == "L'email n'est pas valide.");
        }

        [Fact]
        public void Should_Fail_WhenPasswordIsEmpty()
        {
            //Arrange
            var validator = new LoginCommandValidator();
            //Act
            var result = validator.Validate(new LoginCommand("test@gmail.coml", string.Empty));
            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Password" && e.ErrorMessage == "Le mot de passe est requis.");
        }
    }
}