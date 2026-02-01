using FluentValidation;

namespace BoardGameTools.Application.Library.Games.Commands
{
    public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
    {
        public CreateGameCommandValidator()
        {
            RuleFor(x => x.OwnerId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(x => x.Source)
                .Must(s => s is "manual" or "bgg")
                .WithMessage("La source doit être 'manual' ou 'bgg'")
                .NotEmpty();

            When(x => x.Source == "bgg", () =>
            {
                RuleFor(x => x.SourceGameId)
                    .NotEmpty()
                    .Must(id => int.TryParse(id, out _))
                    .WithMessage("SourceGameId doit être un id BGG valide");
            });
        }
    }
}