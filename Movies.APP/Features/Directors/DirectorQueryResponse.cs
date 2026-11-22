using CORE.Domain;

namespace Movies.APP.Features.Directors
{
    public class DirectorQueryResponse : Record
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsRetired { get; set; }
    }
}
