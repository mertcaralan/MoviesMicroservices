using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Roles
{
    public class RoleRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class RoleRemoveHandler : DbService<Role>,
        IRequestHandler<RoleRemoveRequest, CommandResponse>
    {
        public RoleRemoveHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(RoleRemoveRequest request,
            CancellationToken cancellationToken)
        {
            var roleEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (roleEntity is null)
                return Error("Role not found!");

            await DbRemoveAsync(roleEntity, cancellationToken);

            return Success(roleEntity.Id,
                $"{roleEntity.Name} role deleted successfully.");
        }
    }
}
