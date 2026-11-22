using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Movies.APP.Domain;

namespace Movies.APP.Features.Genres
{
    public class GenreUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(50)]
        public string Name { get; set; }
    }

    public class GenreUpdateHandler : DbService<Genre>,
        IRequestHandler<GenreUpdateRequest, CommandResponse>
    {
        public GenreUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(GenreUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(g => g.Id != request.Id
                && g.Name == request.Name.Trim(), cancellationToken))
                return Error("Genre with the same name exists!");
            var entity = await DbSingleAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Genre not found!");
            entity.Name = request.Name.Trim();
            await DbUpdateAsync(entity, cancellationToken);
            return Success(entity.Id, "Genre updated successfully.");
        }
    }
}
