using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Movies.APP.Domain;

namespace Movies.APP.Features.Movies
{
    public class MovieAddRequest : Record, IRequest<CommandResponse>
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

    public class MovieAddHandler : DbService<Movie>,
        IRequestHandler<MovieAddRequest, CommandResponse>
    {
        private readonly DbContext _db;

        public MovieAddHandler(DbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(MovieAddRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(m => m.Name == request.Name.Trim(), cancellationToken))
                return Error("Movie with the same name exists!");
            if (!await _db.Set<Director>().AnyAsync(d => d.Id == request.DirectorId, cancellationToken))
                return Error("Director not found!");
            var genreIds = request.GenreIds.Distinct().ToList();
            if (await _db.Set<Genre>().CountAsync(g => genreIds.Contains(g.Id), cancellationToken) != genreIds.Count)
                return Error("Genre not found!");
            var entity = new Movie
            {
                Name = request.Name.Trim(),
                ReleaseDate = request.ReleaseDate,
                TotalRevenue = request.TotalRevenue,
                DirectorId = request.DirectorId,
                GenreIds = genreIds
            };
            await DbAddAsync(entity, cancellationToken);
            return Success(entity.Id, "Movie created successfully.");
        }
    }
}
