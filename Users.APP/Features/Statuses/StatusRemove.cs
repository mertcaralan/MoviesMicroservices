using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Statuses
{
    public class StatusRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class StatusRemoveHandler : DbService<Status>,
        IRequestHandler<StatusRemoveRequest, CommandResponse>
    {
        public StatusRemoveHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Status> DbQuery()
        {
            return base.DbQuery().Include(statusEntity => statusEntity.Users);
        }

        public async Task<CommandResponse> Handle(StatusRemoveRequest request,
            CancellationToken cancellationToken)
        {
            var statusEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (statusEntity is null)
                return Error("Status not found!");

            if (statusEntity.Users.Any())
                return Error("Status has relational users!");

            await DbRemoveAsync(statusEntity, cancellationToken);

            return Success(statusEntity.Id, "Status deleted successfully.");
        }
    }
}
