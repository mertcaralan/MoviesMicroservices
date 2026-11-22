using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required, StringLength(15, MinimumLength = 3)]
        public string Password { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required, Range(0, 5)]
        public double? Score { get; set; }

        /*
        The type changed from int to int? to be able to use [Required] attribute.
        If the type is int, the default value will be 0 and the validation will
        always be successful even if no value is assigned.
        */
        [Required]
        public int? StatusId { get; set; }

        [Required]
        public List<int> RoleIds { get; set; }
    }

    public class UserUpdateHandler : DbService<User>,
        IRequestHandler<UserUpdateRequest, CommandResponse>
    {
        public UserUpdateHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<User> DbQuery()
        {
            return base.DbQuery().Include(u => u.UserRoles);
        }

        public async Task<CommandResponse> Handle(UserUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(u => u.Id != request.Id &&
                u.UserName == request.UserName.Trim(), cancellationToken))
                return Error("User with the same user name exists!");

            var userEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (userEntity is null)
                return Error("User not found!");

            if (request.RoleIds is not null)
                DbRemove(userEntity.UserRoles);

            userEntity.UserName = request.UserName.Trim();
            userEntity.Password = request.Password.Trim();
            userEntity.FirstName = request.FirstName?.Trim();
            userEntity.LastName = request.LastName?.Trim();
            userEntity.BirthDate = request.BirthDate;
            userEntity.Gender = request.Gender;
            userEntity.Score = request.Score ?? 0;
            userEntity.StatusId = request.StatusId ?? 0;
            userEntity.RoleIds = request.RoleIds;
            await DbUpdateAsync(userEntity, cancellationToken);

            return Success(userEntity.Id, "User updated successfully!");
        }
    }
}
