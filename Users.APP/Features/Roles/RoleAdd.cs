using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Roles
{
    public class RoleAddRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(25)]
        public string Name { get; set; }

    }

    public class RoleAddHandler : DbService<Role>,
        IRequestHandler<RoleAddRequest, CommandResponse>
    {
        public RoleAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(RoleAddRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(roleEntity =>
                    roleEntity.Name == request.Name.Trim(),
                        cancellationToken))
                return Error(request.Name + " role exists!");

            var roleEntity = new Role
            {
                Name = request.Name.Trim()
            };
            await DbAddAsync(roleEntity, cancellationToken);

            return Success(roleEntity.Id, roleEntity.Name +
                " role created successfully.");
        }
    }
}
