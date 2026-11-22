using CORE.Domain;
using MediatR;

namespace Movies.APP.Features.Directors
{
    public class DirectorQueryRequest : Record, IRequest<IQueryable<DirectorQueryResponse>>
    {
    }
}
