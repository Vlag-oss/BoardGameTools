using FluentValidation;

namespace BoardGameTools.Application.LibraryGames.Queries.GetLibraryGames
{
    public class GetLibraryGamesQueryValidator : AbstractValidator<GetLibraryGamesQuery>
    {
        public GetLibraryGamesQueryValidator()
        {
            RuleFor(x => x.OwnerId).NotEmpty();
        }
    }
}
