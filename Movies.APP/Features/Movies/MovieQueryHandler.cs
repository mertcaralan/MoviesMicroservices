using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Movies
{
    public class MovieQueryHandler : DbService<Movie>,
        IRequestHandler<MovieQueryRequest, IQueryable<MovieQueryResponse>>
    {
        public MovieQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Movie> DbQuery()
        {
            return base.DbQuery().Include(m => m.Director)
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                .OrderBy(m => m.Name);
        }

        public Task<IQueryable<MovieQueryResponse>> Handle(MovieQueryRequest request,
            CancellationToken cancellationToken)
        {
            var query = DbQuery().Select(m => new MovieQueryResponse
            {
                Id = m.Id,
                Name = m.Name,
                ReleaseDate = m.ReleaseDate,
                TotalRevenue = m.TotalRevenue,
                DirectorId = m.DirectorId,
                GenreIds = m.MovieGenres.Select(mg => mg.GenreId).ToList(),
                ReleaseDateR = m.ReleaseDate.HasValue
                    ? m.ReleaseDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                TotalRevenueR = m.TotalRevenue.ToString("N2"),
                DirectorFullNameR = m.Director.FirstName + " " + m.Director.LastName,
                GenreNamesR = string.Join(", ", m.MovieGenres.Select(mg => mg.Genre.Name))
            });
            return Task.FromResult(query);
        }
    }
}
