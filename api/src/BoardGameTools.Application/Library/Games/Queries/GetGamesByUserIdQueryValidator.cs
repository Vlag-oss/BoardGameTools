using FluentValidation;

namespace BoardGameTools.Application.Library.Games.Queries
{
    public class GetGamesByUserIdQueryValidator : AbstractValidator<GetGamesByUserIdQuery>
    {
        public GetGamesByUserIdQueryValidator()
        {
            RuleFor(x => x.OwnerId).NotEmpty();
        }
    }
}
