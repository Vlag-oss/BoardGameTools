using BoardGameTools.Application.Users.Commands.AddUser;

namespace BoardGameTools.UnitTests.Users
{
    public class AddUserCommandValidatorTests
    {
        [Fact]
        public void Should_HaveErrorWhenEmailIsEmpty()
        {
            // Arrange
            var validator = new AddUserCommandValidator();
            var command = new AddUserCommand(string.Empty, "Password123!", "Password123!");

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Email" && e.ErrorMessage == "L'email est requis.");
        }

        [Fact]
        public void Should_HaveErrorWhenEmailIsInvalid()
        {
            //Arrange
            var validator = new AddUserCommandValidator();
            var command = new AddUserCommand("invalid-email", "Password123!", "Password123!");

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Email" && e.ErrorMessage == "L'email n'est pas valide.");
        }

        [Fact]
        public void Should_HaveErrorWhenPasswordIsEmpty()
        {
            // Arrange
            var validator = new AddUserCommandValidator();
            var command = new AddUserCommand("test@gmail.com", string.Empty, "Password123!");

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorMessage == "Le mot de passe est requis.");
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
            var validator = new AddUserCommandValidator();
            var command = new AddUserCommand("test@gmail.com", password, password);

            //Act
            var result = validator.Validate(command);

            //Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public void Should_HaveErrorWhenPasswordsDoNotMatch()
        {
            // Arrange
            var validator = new AddUserCommandValidator();
            var command = new AddUserCommand("test@gmail.com", "Password123!", "DifferentPassword");

            //Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "ConfirmPassword" && e.ErrorMessage == "Les mots de passe ne correspondent pas.");
        }
    }
}
