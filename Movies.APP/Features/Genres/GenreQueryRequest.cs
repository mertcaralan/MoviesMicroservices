using CORE.Domain;
using MediatR;

namespace Movies.APP.Features.Genres
{
    public class GenreQueryRequest : Record, IRequest<IQueryable<GenreQueryResponse>>
    {
    }
}
