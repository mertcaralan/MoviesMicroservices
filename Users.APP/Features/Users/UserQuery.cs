using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;
using Users.APP.Features.Roles;
using Users.APP.Features.Statuses;

namespace Users.APP.Features.Users
{
    public class UserQueryRequest : Record,
        IRequest<IQueryable<UserQueryResponse>>
    {
    }

    public class UserQueryResponse : Record
    {
        public string UserName { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public double Score { get; set; }

        public bool IsOnline { get; set; }

        public int StatusId { get; set; }

        public string IsOnlineR { get; set; }

        public string ScoreR { get; set; }

        public string RegistrationDateR { get; set; }

        public string BirthDateR { get; set; }

        public string GenderR { get; set; }

        public string FullNameR { get; set; }

        public string PasswordR { get; set; }

        public string StatusTitleR { get; set; }

        public StatusQueryResponse StatusR { get; set; }

        public string RoleNamesR { get; set; }

        public List<RoleQueryResponse> RolesR { get; set; }
    }

    public class UserQueryHandler : DbService<User>,
        IRequestHandler<UserQueryRequest,
            IQueryable<UserQueryResponse>>
    {
        public UserQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<User> DbQuery()
        {
            return base.DbQuery()
                .Include(u => u.Status)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .OrderByDescending(u => u.Score)
                .ThenByDescending(u => u.RegistrationDate)
                .ThenBy(u => u.UserName);
        }

        public Task<IQueryable<UserQueryResponse>> Handle(
            UserQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                DbQuery().Select(u => new UserQueryResponse
            {
                Id = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                RegistrationDate = u.RegistrationDate,
                BirthDate = u.BirthDate,
                IsOnline = u.IsOnline,
                Gender = u.Gender,
                Score = u.Score,
                StatusId = u.StatusId,
                StatusTitleR = u.Status.Title,
                StatusR = new StatusQueryResponse
                {
                    Id = u.Status.Id,
                    Title = u.Status.Title
                },
                RoleNamesR = string.Join(", ", u.UserRoles
                     .OrderBy(ur => ur.Role.Name)
                     .Select(ur => ur.Role.Name)),
                RolesR = u.UserRoles.OrderBy(ur => ur.Role.Name)
                     .Select(ur => new RoleQueryResponse
                     {
                         Id = ur.Role.Id,
                         Name = ur.Role.Name
                     }).ToList(),
                PasswordR = new string('*', u.Password.Length),
                FullNameR = u.FirstName + " " + u.LastName,
                RegistrationDateR = u.RegistrationDate
                     .ToString("MM/dd/yyyy HH:mm.ss"),
                BirthDateR = u.BirthDate.HasValue ?
                     u.BirthDate.Value.ToShortDateString() :
                     string.Empty,
                IsOnlineR = u.IsOnline ? "Yes" : "No",
                GenderR = u.Gender.ToString(),
                ScoreR = u.Score.ToString("N1")
            }));
        }
    }
}
