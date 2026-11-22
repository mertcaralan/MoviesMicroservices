using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Movies.APP.Domain;

namespace Movies.APP.Features.Directors
{
    public class DirectorAddRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        public bool IsRetired { get; set; }
    }

    public class DirectorAddHandler : DbService<Director>,
        IRequestHandler<DirectorAddRequest, CommandResponse>
    {
        public DirectorAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(DirectorAddRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(d => d.FirstName == request.FirstName.Trim()
                && d.LastName == request.LastName.Trim(), cancellationToken))
                return Error("Director with the same name exists!");
            var entity = new Director
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                IsRetired = request.IsRetired
            };
            await DbAddAsync(entity, cancellationToken);
            return Success(entity.Id, "Director created successfully.");
        }
    }
}
