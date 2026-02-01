using BoardGameTools.Application.Services.Passwords;
using FluentValidation;

namespace BoardGameTools.Application.Auth.Register.Commands
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("L'email est requis.")
                .EmailAddress().WithMessage("L'email n'est pas valide.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Le mot de passe est requis.")
                .Custom((password, context) =>
                {
                    var errors = PasswordPolicy.Validate(password);
                    foreach (var error in errors)
                        context.AddFailure(nameof(context.InstanceToValidate.Password), error);
                });

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Les mots de passe ne correspondent pas.");
        }
    }
}
