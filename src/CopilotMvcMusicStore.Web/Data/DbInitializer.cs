using CopilotMvcMusicStore.Web.Models;

namespace CopilotMvcMusicStore.Web.Data
{
    public class DbInitializer
    {
        /*
           Initilize method that accepts musicstorecontext type context
           This method should do the following:
            1. generate an empty database schema if it doesn't exist
            2. add check to see if the DB already contains albums, genres, or artists. If it does return early.
            3. Generate 20 genres, 20 artists, 10 albums and then commit them to the DB. Commit the array for each object type to the DB before you create 
                the next array. This will ensure that the foreign key relationships are created properly.
         */
        public static void Initialize(MusicStoreContext context)
        {
            context.Database.EnsureCreated();

            if (context.Albums.Any() || context.Genres.Any() || context.Artists.Any())
            {
                return;
            }

            var genres = new Genre[]
            {
                new Genre { Name = "Disco" },
                new Genre { Name = "Jazz" },
                new Genre { Name = "Rock" }
            };
            context.Genres.AddRange(genres);
            context.SaveChanges();

            var artists = new Artist[]
            {
                new Artist { Name = "Artist 1" },
                new Artist { Name = "Artist 2" },
                new Artist { Name = "Artist 3" }
            };
            context.Artists.AddRange(artists);
            context.SaveChanges();

            var albums = new Album[]
            {
                new Album { Title = "Album 1", ArtistId = artists[0].ArtistId, GenreId = genres[0].GenreId, Price = 5.69m },
                new Album { Title = "Album 1 A", ArtistId = artists[0].ArtistId, GenreId = genres[0].GenreId, Price = 9.99m },
                new Album { Title = "Album 2", ArtistId = artists[1].ArtistId, GenreId = genres[1].GenreId, Price = 20.99m },
                new Album { Title = "Album 3", ArtistId = artists[2].ArtistId, GenreId = genres[2].GenreId, Price = 9.99m },
                new Album { Title = "Album 3 A", ArtistId = artists[2].ArtistId, GenreId = genres[2].GenreId, Price = 2.99m },
            };
            context.Albums.AddRange(albums);
            context.SaveChanges();
        }
    }
}
