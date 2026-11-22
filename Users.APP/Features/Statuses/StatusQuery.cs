using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;
using Users.APP.Features.Users;

namespace Users.APP.Features.Statuses
{
    public class StatusQueryRequest : Record,
        IRequest<IQueryable<StatusQueryResponse>>
    {
    }

    public class StatusQueryResponse : Record
    {
        public string Title { get; set; }

        public string UserNamesR { get; set; }

        public int UserCountR { get; set; }

        public List<UserQueryResponse> UsersR { get; set; }
    }

    public class StatusQueryHandler : DbService<Status>,
        IRequestHandler<StatusQueryRequest, IQueryable<StatusQueryResponse>>
    {
        public StatusQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Status> DbQuery()
        {
            return base.DbQuery()
                .Include(s => s.Users)
                .OrderBy(s => s.Title);
        }

        public Task<IQueryable<StatusQueryResponse>> Handle(
            StatusQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbQuery().Select(s => new StatusQueryResponse
            {
                Id = s.Id,
                Title = s.Title,
                UserNamesR = string.Join(", ", s.Users.Select(u => u.UserName)),
                UserCountR = s.Users.Count,
                UsersR = s.Users.Select(u => new UserQueryResponse
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    PasswordR = new string('*', u.Password.Length),
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    FullNameR = u.FirstName + " " + u.LastName,
                    RegistrationDate = u.RegistrationDate,
                    BirthDate = u.BirthDate,
                    Gender = u.Gender,
                    IsOnline = u.IsOnline,
                    Score = u.Score,
                    StatusId = u.StatusId
                }).ToList()
            });

            return Task.FromResult(query);
        }
    }
}
