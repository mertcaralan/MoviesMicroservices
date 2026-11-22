using CORE.Domain;
using MediatR;

namespace Movies.APP.Features.Movies
{
    public class MovieQueryRequest : Record, IRequest<IQueryable<MovieQueryResponse>>
    {
    }
}
