using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.Application.Library.Games.Commands
{
    public record CreateGameCommand(Guid OwnerId, string Name, string Source, string? SourceGameId) : IRequest<Guid>;

    public class CreateGameCommandHandler(IAppDbContext context) : IRequestHandler<CreateGameCommand, Guid>
    {
        private readonly IAppDbContext _context = context;

        public async Task<Guid> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var game = request.Source.ToLower() == "bgg"
                ? Game.CreateFromBgg(request.OwnerId, request.Name, int.Parse(request.SourceGameId!))
                : Game.CreateManual(request.OwnerId, request.Name);
             
            var exists = await _context.Games
                .AsNoTracking()
                .AnyAsync(g => g.OwnerId == request.OwnerId && g.Source == request.Source && g.Name == request.Name, cancellationToken);

            if(exists) throw new InvalidOperationException("Le jeu existe déjà dans votre library");

            _context.Games.Add(game);
            await _context.SaveChangesAsync(cancellationToken);

            return game.Id;
        }
    }
}
