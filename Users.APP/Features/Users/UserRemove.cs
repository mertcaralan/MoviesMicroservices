using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class UserRemoveHandler : DbService<User>,
        IRequestHandler<UserRemoveRequest, CommandResponse>
    {
        public UserRemoveHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<User> DbQuery()
        {
            return base.DbQuery().Include(u => u.UserRoles);
        }

        public async Task<CommandResponse> Handle(UserRemoveRequest request,
            CancellationToken cancellationToken)
        {
            var userEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (userEntity is null)
                return Error("User not found!");

            DbRemove(userEntity.UserRoles);

            await DbRemoveAsync(userEntity, cancellationToken);

            return Success(userEntity.Id, "User deleted successfully.");
        }
    }
}
