using CORE.Domain;

namespace Movies.APP.Features.Movies
{
    public class MovieQueryResponse : Record
    {
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal TotalRevenue { get; set; }

        public int DirectorId { get; set; }

        public List<int> GenreIds { get; set; }

        public string ReleaseDateR { get; set; }

        public string TotalRevenueR { get; set; }

        public string DirectorFullNameR { get; set; }

        public string GenreNamesR { get; set; }
    }
}
