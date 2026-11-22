using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Movies
{
    public class MovieRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class MovieRemoveHandler : DbService<Movie>,
        IRequestHandler<MovieRemoveRequest, CommandResponse>
    {
        public MovieRemoveHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Movie> DbQuery()
        {
            return base.DbQuery().Include(m => m.MovieGenres);
        }

        public async Task<CommandResponse> Handle(MovieRemoveRequest request,
            CancellationToken cancellationToken)
        {
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Movie not found!");
            DbRemove(entity.MovieGenres);
            await DbRemoveAsync(entity, cancellationToken);
            return Success(entity.Id, "Movie removed successfully.");
        }
    }
}
