using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Statuses
{
    public class StatusAddRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(10)]
        public string Title { get; set; }
    }

    public class StatusAddHandler : DbService<Status>,
        IRequestHandler<StatusAddRequest, CommandResponse>
    {
        public StatusAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(StatusAddRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(statusEntity =>
                    statusEntity.Title == request.Title.Trim(),
                        cancellationToken))
                return Error("Status with the same title exists!");

            var statusEntity = new Status
            {
                Title = request.Title.Trim()
            };
            await DbAddAsync(statusEntity, cancellationToken);

            return Success(statusEntity.Id, "Status created successfully.");
        }
    }
}
