using BoardGameTools.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BoardGameTools.Application.LibraryGames.Queries.GetLibraryGames
{
    public record GetLibraryGamesQuery(Guid OwnerId) : IRequest<List<LibraryGameDto>>;

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
