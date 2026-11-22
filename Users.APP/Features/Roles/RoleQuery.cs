using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Roles
{
    public class RoleQueryRequest : Record,
        IRequest<IQueryable<RoleQueryResponse>>
    {
    }

    public class RoleQueryResponse : Record
    {
        public string Name { get; set; }
    }

    public class RoleQueryHandler : DbService<Role>,
        IRequestHandler<RoleQueryRequest, IQueryable<RoleQueryResponse>>
    {
        public RoleQueryHandler(DbContext db) : base(db)
        {
        }

        public Task<IQueryable<RoleQueryResponse>> Handle(
            RoleQueryRequest request, CancellationToken cancellationToken)
        {
            var query =
                DbQuery()
                .Select(roleEntity => new RoleQueryResponse()
                {
                    Id = roleEntity.Id,
                    Name = roleEntity.Name
                });

            return Task.FromResult(query);
        }
    }
}
