using BoardGameTools.Application.Password.Commands;
using FluentAssertions;

namespace BoardGameTools.UnitTests.Password
{
    public class ForgotPasswordCommandValidatorTests
    {
        [Fact]
        public void Should_Fail_WhenEmailIsEmpty()
        {
            //Arrange
            var validator = new ForgotPasswordCommandValidator();
            var command = new ForgotPasswordCommand(string.Empty);

            //Act
            var result = validator.Validate(command);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage.Contains("L'email est requis."));
        }

        [Fact]
        public void Should_Fail_WhenEmailIsInvalid()
        {
            //Arrange
            var validator = new ForgotPasswordCommandValidator();
            var command = new ForgotPasswordCommand("test.email");

            //Act
            var result = validator.Validate(command);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email" && e.ErrorMessage.Contains("L'email n'est pas valide."));
        }
    }
}
