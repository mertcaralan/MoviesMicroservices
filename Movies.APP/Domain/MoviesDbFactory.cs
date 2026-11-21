using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Movies.APP.Domain
{
    public class MoviesDbFactory : IDesignTimeDbContextFactory<MoviesDb>
    {
        const string CONNECTIONSTRING = "data source=MoviesDB.db";

        public MoviesDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MoviesDb>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new MoviesDb(optionsBuilder.Options);
        }

        public void SeedDb()
        {
            using var db = CreateDbContext(null);
            using var transaction = db.Database.BeginTransaction();

            db.MovieGenres.RemoveRange(db.MovieGenres.ToList());
            db.Movies.RemoveRange(db.Movies.ToList());
            db.Directors.RemoveRange(db.Directors.ToList());
            db.Genres.RemoveRange(db.Genres.ToList());
            db.SaveChanges();

            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='MovieGenres'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Movies'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Directors'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Genres'");

            db.Genres.Add(new Genre { Name = "Drama" });
            db.Genres.Add(new Genre { Name = "Science Fiction" });
            db.Genres.Add(new Genre { Name = "Adventure" });
            db.SaveChanges();

            db.Directors.Add(new Director
            {
                FirstName = "Christopher",
                LastName = "Nolan",
                IsRetired = false,
                Movies = new List<Movie>
                {
                    new Movie
                    {
                        Name = "Interstellar",
                        ReleaseDate = new DateTime(2014, 11, 7),
                        TotalRevenue = 700000000,
                        GenreIds = db.Genres.Select(g => g.Id).ToList()
                    }
                }
            });
            db.Directors.Add(new Director
            {
                FirstName = "Frank",
                LastName = "Darabont",
                IsRetired = true,
                Movies = new List<Movie>
                {
                    new Movie
                    {
                        Name = "The Shawshank Redemption",
                        ReleaseDate = new DateTime(1994, 9, 23),
                        TotalRevenue = 73000000,
                        GenreIds = db.Genres.Where(g => g.Name == "Drama")
                            .Select(g => g.Id).ToList()
                    }
                }
            });
            db.SaveChanges();
            transaction.Commit();
        }
    }
}
