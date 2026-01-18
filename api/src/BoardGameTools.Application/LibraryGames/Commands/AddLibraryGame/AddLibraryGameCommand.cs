using MediatR;

namespace BoardGameTools.Application.LibraryGames.Commands.AddLibraryGame
{
    public record AddLibraryGameCommand(Guid OwnerId, string Name, string Source, string? SourceGameId) : IRequest<Guid>;
}
