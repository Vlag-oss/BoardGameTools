using BoardGameTools.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BoardGameTools.Application.LibraryGames.Commands.GetLibraryGames
{
    public class GetLibraryGamesQueryHandler(IAppDbContext context) : IRequestHandler<GetLibraryGamesQuery, List<LibraryGameDto>>
    {
        private readonly IAppDbContext _context = context;

        public async Task<List<LibraryGameDto>> Handle(GetLibraryGamesQuery request, CancellationToken cancellationToken)
            => await _context.LibraryGames
                    .Where(g => g.OwnerId == request.OwnerId)
                    .Select(g => new LibraryGameDto
                    {
                        Id = g.Id,
                        Name = g.Name
                    })
                    .ToListAsync(cancellationToken);
    }
}
