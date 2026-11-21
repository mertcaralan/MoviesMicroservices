using CORE.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.APP.Domain
{
    public class Movie : Record
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal TotalRevenue { get; set; }

        public int DirectorId { get; set; } // foreign key

        public Director Director { get; set; } // navigation property

        public List<MovieGenre> MovieGenres { get; set; } = new();

        [NotMapped]
        public List<int> GenreIds
        {
            get => MovieGenres.Select(mg => mg.GenreId).ToList();
            set => MovieGenres = value.Select(gId => new MovieGenre
            {
                GenreId = gId
            }).ToList();
        }
    }
}
