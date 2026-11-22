using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Genres
{
    public class GenreRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class GenreRemoveHandler : DbService<Genre>,
        IRequestHandler<GenreRemoveRequest, CommandResponse>
    {
        public GenreRemoveHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Genre> DbQuery()
        {
            return base.DbQuery().Include(g => g.MovieGenres);
        }

        public async Task<CommandResponse> Handle(GenreRemoveRequest request,
            CancellationToken cancellationToken)
        {
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Genre not found!");
            if (entity.MovieGenres.Any())
                return Error("Genre can't be deleted because it has relational movies!");
            await DbRemoveAsync(entity, cancellationToken);
            return Success(entity.Id, "Genre removed successfully.");
        }
    }
}
