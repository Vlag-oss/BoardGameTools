using BoardGameTools.Application.Interfaces;
using BoardGameTools.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.Application.LibraryGames.Commands.AddLibraryGame
{
    public class AddLibraryGameCommandHandler(IAppDbContext context) : IRequestHandler<AddLibraryGameCommand, Guid>
    {
        private readonly IAppDbContext _context = context;

        public async Task<Guid> Handle(AddLibraryGameCommand request, CancellationToken cancellationToken)
        {
            var libraryGame = request.Source.ToLower() switch
            {
                "bgg" when int.TryParse(request.SourceGameId, out var bggId) =>
                    LibraryGame.CreateFromBgg(request.OwnerId, request.Name, bggId),
                "manual" =>
                    LibraryGame.CreateManual(request.OwnerId, request.Name),
                _ => throw new ArgumentException("Invalid source or source game ID.")
            };

            var exists = await _context.LibraryGames
                .AsNoTracking()
                .AnyAsync(g => g.OwnerId == request.OwnerId && g.Source == request.Source && g.Name == request.Name, cancellationToken);

            if(exists) throw new InvalidOperationException("Le jeu existe déjà dans votre library");

            _context.LibraryGames.Add(libraryGame);
            await _context.SaveChangesAsync(cancellationToken);

            return libraryGame.Id;
        }
    }
}
