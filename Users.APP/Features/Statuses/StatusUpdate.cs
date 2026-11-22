using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Statuses
{
    public class StatusUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(10)]
        public string Title { get; set; }
    }

    public class StatusUpdateHandler : DbService<Status>,
        IRequestHandler<StatusUpdateRequest, CommandResponse>
    {
        public StatusUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(StatusUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(statusEntity => statusEntity.Id != request.Id &&
                    statusEntity.Title == request.Title.Trim(),
                        cancellationToken))
                return Error("Status with the same title exists!");

            var statusEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (statusEntity is null)
                return Error("Status not found!");

            statusEntity.Title = request.Title.Trim();
            await DbUpdateAsync(statusEntity, cancellationToken);

            return Success(statusEntity.Id, "Status updated successfully.");
        }
    }
}
