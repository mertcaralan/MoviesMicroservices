using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Roles
{
    public class RoleUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(25)]
        public string Name { get; set; }

    }

    public class RoleUpdateHandler : DbService<Role>,
        IRequestHandler<RoleUpdateRequest, CommandResponse>
    {
        public RoleUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(RoleUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(roleEntity => roleEntity.Id != request.Id &&
                    roleEntity.Name == request.Name.Trim(),
                        cancellationToken))
                return Error($"{request.Name} role exists!");

            var roleEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (roleEntity is null)
                return Error("Role not found!");

            roleEntity.Name = request.Name.Trim();
            await DbUpdateAsync(roleEntity, cancellationToken);

            return Success(roleEntity.Id,
                $"{roleEntity.Name} role updated successfully.");
        }
    }
}
