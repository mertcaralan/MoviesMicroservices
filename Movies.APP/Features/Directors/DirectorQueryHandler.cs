using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Directors
{
    public class DirectorQueryHandler : DbService<Director>,
        IRequestHandler<DirectorQueryRequest, IQueryable<DirectorQueryResponse>>
    {
        public DirectorQueryHandler(DbContext db) : base(db)
        {
        }

        public Task<IQueryable<DirectorQueryResponse>> Handle(DirectorQueryRequest request,
            CancellationToken cancellationToken)
        {
            var entityQuery = DbQuery();
            var responseQuery = entityQuery.Select(directorEntity => new DirectorQueryResponse
            {
                Id = directorEntity.Id,
                FirstName = directorEntity.FirstName,
                LastName = directorEntity.LastName,
                IsRetired = directorEntity.IsRetired
            });
            return Task.FromResult(responseQuery);
        }
    }
}
