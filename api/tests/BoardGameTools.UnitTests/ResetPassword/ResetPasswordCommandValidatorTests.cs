using BoardGameTools.Application.Auth.ResetPassword;

namespace BoardGameTools.UnitTests.ResetPassword
{
    public class ResetPasswordCommandValidatorTests
    {
        [Fact]
        public void Should_HaveErrorWhenTokenIsEmpty()
        {
            // Arrange
            var validator = new ResetPasswordCommandValidator();
            var command = new ResetPasswordCommand(string.Empty, string.Empty, string.Empty);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Token" && e.ErrorMessage == "Le token est requis.");
        }

        [Fact]
        public void Should_HaveErrorWhenNewPasswordIsEmpty()
        {
            // Arrange
            var validator = new ResetPasswordCommandValidator();
            var command = new ResetPasswordCommand("123", string.Empty, string.Empty);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "NewPassword" && e.ErrorMessage == "Le nouveau mot de passe est requis.");
        }

        [Theory]
        [InlineData("short", "Le mot de passe doit contenir au moins 8 caractères.")]
        [InlineData("alllowercase1!", "Le mot de passe doit contenir au moins une lettre majuscule.")]
        [InlineData("ALLUPPERCASE1!", "Le mot de passe doit contenir au moins une lettre minuscule.")]
        [InlineData("NoNumbers!", "Le mot de passe doit contenir au moins un chiffre.")]
        [InlineData("NoSpecialChar1", "Le mot de passe doit contenir au moins un caractère spécial.")]
        public void Should_HaveErrorWhenPasswordDoesNotMeetCriteria(string password, string expectedErrorMessage)
        {
            // Arrange
            var validator = new ResetPasswordCommandValidator();
            var command = new ResetPasswordCommand("123", password, password);

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "NewPassword" && e.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public void Should_HaveErrorWhenPasswordsDoNotMatch()
        {
            // Arrange
            var validator = new ResetPasswordCommandValidator();
            var command = new ResetPasswordCommand("123", "Password123!", "DifferentPassword");

            //Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "ConfirmPassword" && e.ErrorMessage == "Les mots de passe ne correspondent pas.");
        }
    }
}
