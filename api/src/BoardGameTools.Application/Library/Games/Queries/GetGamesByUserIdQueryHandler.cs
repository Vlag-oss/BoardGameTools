using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Library.Games.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BoardGameTools.Application.Library.Games.Queries
{
    public record GetGamesByUserIdQuery(Guid OwnerId) : IRequest<List<LibraryGameDto>>;

    public class GetGamesByUserIdQueryHandler(IAppDbContext context) : IRequestHandler<GetGamesByUserIdQuery, List<LibraryGameDto>>
    {
        private readonly IAppDbContext _context = context;

        public async Task<List<LibraryGameDto>> Handle(GetGamesByUserIdQuery request, CancellationToken cancellationToken)
            => await _context.Games
                    .Where(g => g.OwnerId == request.OwnerId)
                    .Select(g => new LibraryGameDto
                    {
                        Id = g.Id,
                        Name = g.Name
                    })
                    .ToListAsync(cancellationToken);
    }
}
