using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Movies.APP.Domain;

namespace Movies.APP.Features.Movies
{
    public class MovieUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalRevenue { get; set; }

        [Range(1, int.MaxValue)]
        public int DirectorId { get; set; }

        [Required]
        public List<int> GenreIds { get; set; } = new();
    }

    public class MovieUpdateHandler : DbService<Movie>,
        IRequestHandler<MovieUpdateRequest, CommandResponse>
    {
        private readonly DbContext _db;

        public MovieUpdateHandler(DbContext db) : base(db)
        {
            _db = db;
        }

        protected override IQueryable<Movie> DbQuery()
        {
            return base.DbQuery().Include(m => m.MovieGenres);
        }

        public async Task<CommandResponse> Handle(MovieUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(m => m.Id != request.Id
                && m.Name == request.Name.Trim(), cancellationToken))
                return Error("Movie with the same name exists!");
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Movie not found!");
            if (!await _db.Set<Director>().AnyAsync(d => d.Id == request.DirectorId, cancellationToken))
                return Error("Director not found!");
            var genreIds = request.GenreIds.Distinct().ToList();
            if (await _db.Set<Genre>().CountAsync(g => genreIds.Contains(g.Id), cancellationToken) != genreIds.Count)
                return Error("Genre not found!");
            DbRemove(entity.MovieGenres);
            entity.Name = request.Name.Trim();
            entity.ReleaseDate = request.ReleaseDate;
            entity.TotalRevenue = request.TotalRevenue;
            entity.DirectorId = request.DirectorId;
            entity.GenreIds = genreIds;
            await DbUpdateAsync(entity, cancellationToken);
            return Success(entity.Id, "Movie updated successfully.");
        }
    }
}
