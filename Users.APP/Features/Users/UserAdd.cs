using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Users
{
    public class UserAddRequest : Record, IRequest<CommandResponse>
    {
        /*
        ErrorMessage parameter can be set in all data annotations
        to show custom validation error messages:
        Example 1: [Required(ErrorMessage = "{0} is required!")]
        where {0} is the property name which is "UserName".
        Example 2: [StringLength(30, 3,
        ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        where {0} is the property name which is "UserName", {1} is the
        first parameter which is 30 and {2} is the second parameter
        which is 3.
        */
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(30, MinimumLength = 3,
            ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, MinimumLength = 3,
            ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Password { get; set; }

        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        /*
        The type changed from double to double? to be able to use [Required]
        attribute. If the type is double, the default value will be 0 and
        the validation will always be successful even if no value is assigned.
        */
        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, 5, ErrorMessage = "{0} must be between {1} and {2}!")]
        public double? Score { get; set; }

        /*
        The type changed from int to int? to be able to use [Required] attribute.
        If the type is int, the default value will be 0 and the validation will
        always be successful even if no value is assigned.
        */
        [Required(ErrorMessage = "{0} is required!")]
        public int? StatusId { get; set; }

        [Required(ErrorMessage = "At least one role is required!")]
        public List<int> RoleIds { get; set; }
    }

    public class UserAddHandler : DbService<User>,
        IRequestHandler<UserAddRequest, CommandResponse>
    {
        public UserAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(
            UserAddRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(u =>
                u.UserName == request.UserName.Trim(), cancellationToken
            ))
                return Error("User with the same user name exists!");

            var userEntity = new User
            {
                UserName = request.UserName.Trim(),
                Password = request.Password.Trim(),
                FirstName = request.FirstName?.Trim(),
                LastName = request.LastName?.Trim(),
                RegistrationDate = DateTime.Now,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                Score = request.Score ?? 0,
                StatusId = request.StatusId ?? 0,
                RoleIds = request.RoleIds
            };
            await DbAddAsync(userEntity, cancellationToken);

            return Success(userEntity.Id, "User created successfully.");
        }
    }
}
