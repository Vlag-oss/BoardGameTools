using MediatR;

namespace BoardGameTools.Application.LibraryGames.Commands.GetLibraryGames
{
    public record GetLibraryGamesQuery(Guid OwnerId) : IRequest<List<LibraryGameDto>>;
}
