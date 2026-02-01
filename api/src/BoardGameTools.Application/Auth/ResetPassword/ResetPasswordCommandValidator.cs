using BoardGameTools.Application.Services.Passwords;
using FluentValidation;

namespace BoardGameTools.Application.Auth.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Le token est requis.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Le nouveau mot de passe est requis.")
                .Custom((password, context) =>
                {
                    var errors = PasswordPolicy.Validate(password);
                    foreach (var error in errors)
                        context.AddFailure(nameof(context.InstanceToValidate.NewPassword), error);
                });

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Les mots de passe ne correspondent pas.");
        }
    }
}