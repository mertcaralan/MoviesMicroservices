using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Genres
{
    public class GenreQueryHandler : DbService<Genre>,
        IRequestHandler<GenreQueryRequest, IQueryable<GenreQueryResponse>>
    {
        public GenreQueryHandler(DbContext db) : base(db)
        {
        }

        public Task<IQueryable<GenreQueryResponse>> Handle(GenreQueryRequest request,
            CancellationToken cancellationToken)
        {
            var entityQuery = DbQuery();
            var responseQuery = entityQuery.Select(genreEntity => new GenreQueryResponse
            {
                Id = genreEntity.Id,
                Name = genreEntity.Name
            });
            return Task.FromResult(responseQuery);
        }
    }
}
