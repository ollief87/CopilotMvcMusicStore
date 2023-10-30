using CopilotMvcMusicStore.Web.Models;
using CopilotMvcMusicStore.Web.Controllers;
using CopilotMvcMusicStore.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CopilotMvcMusicStore.Test
{
    public class StoreManagerControllerTests
    {
        // Fields for the ILogger<StoreManagerController> and MusicStoreContext
        private readonly ILogger<StoreManagerController> _logger;
        private readonly DbContextOptions<MusicStoreContext> _options;

        // Constructor that initializes the fields
        public StoreManagerControllerTests()
        {
            // Mock the ILogger<StoreManagerController> using NSubstitute
            _logger = Substitute.For<ILogger<StoreManagerController>>();
            _options = new DbContextOptionsBuilder<MusicStoreContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        // Create a test for the Index method
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an album with genre and artist to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Index method.
        // Assert: Assert that the Index method returns a ViewResult and associated data
        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new album & related genre of rock to the DB
                var album = new Album { Title = "Album 1", Artist = new Artist { Name = "Artist 1" }, Genre = new Genre { Name = "Rock" } };
                context.Albums.Add(album);
                context.SaveChanges();

                var controller = new StoreManagerController(context, _logger);

                // Act
                var result = controller.Index();

                // Assert is type view result, and model has count 1, with the first item being the album, with artist and genre included
                Assert.IsType<ViewResult>(result);
                var viewResult = result as ViewResult;
                Assert.NotNull(viewResult);

                Assert.IsType<List<Album>>(viewResult.Model);
                var model = viewResult.Model as List<Album>;
                Assert.NotNull(model);
                Assert.Single(model);
                
                Assert.Equal(album, model[0]);
                Assert.Equal(album.Title, model[0].Title);
                Assert.NotNull(model[0].Artist);
                Assert.Equal(album.Artist.Name, model[0].Artist!.Name);
                Assert.NotNull(model[0].Genre);
                Assert.Equal(album.Genre.Name, model[0].Genre!.Name);
            }
        }

        // Create a test for the Create method
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an artist and genre to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Create method.
        // Assert: Assert that the Create method returns a ViewResult, along with populating the ViewBag.ArtistId and ViewBag.GenreId properties
        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new artist and genre to the DB
                var artist = new Artist { Name = "Artist 1" };
                var genre = new Genre { Name = "Rock" };
                context.Artists.Add(artist);
                context.Genres.Add(genre);
                context.SaveChanges();

                var controller = new StoreManagerController(context, _logger);

                // Act
                var result = controller.Create();

                // Assert
                Assert.IsType<ViewResult>(result);
                var viewResult = result as ViewResult;
                Assert.NotNull(viewResult);
                Assert.NotNull(controller.ViewBag.ArtistId);
                Assert.NotNull(controller.ViewBag.GenreId);
                Assert.Equal(artist.ArtistId.ToString(), ((SelectList)controller.ViewBag.ArtistId).ElementAt(0).Value);
                Assert.Equal(genre.GenreId.ToString(), ((SelectList)controller.ViewBag.GenreId).ElementAt(0).Value);
                Assert.Equal(artist.Name, ((SelectList)controller.ViewBag.ArtistId).ElementAt(0).Text);
                Assert.Equal(genre.Name, ((SelectList)controller.ViewBag.GenreId).ElementAt(0).Text);
            }
        }

        // Create a test for the Create method that accepts an album model - valid model should save to the DB and redirect to Index
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an artist and genre to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Create method with a valid album model.
        // Assert: Assert that the Create method returns a RedirectToActionResult, and that the album was added to the DB
        [Fact]
        public void Create_ValidModel_RedirectsToIndex()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new artist and genre to the DB
                var artist = new Artist { Name = "Artist 1" };
                var genre = new Genre { Name = "Rock" };
                context.Artists.Add(artist);
                context.Genres.Add(genre);
                context.SaveChanges();

                var controller = new StoreManagerController(context, _logger);

                // Act
                var result = controller.Create(new Album { Title = "Album 1", ArtistId = artist.ArtistId, GenreId = genre.GenreId, Price = 9.99m });

                // Assert
                Assert.IsType<RedirectToActionResult>(result);
                var redirectResult = result as RedirectToActionResult;
                Assert.NotNull(redirectResult);
                Assert.Equal("Index", redirectResult.ActionName);
                Assert.Equal("StoreManager", redirectResult.ControllerName);
                Assert.True(context.Albums.Any(a => a.Title == "Album 1"));
            }
        }

        // Create a test for the Create method that accepts an album model - invalid model should return the view with the model
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an artist and genre to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Create method with an invalid album model.
        // Assert: Assert that the Create method returns a ViewResult, and that the album was not added to the DB
        [Fact]
        public void Create_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new artist and genre to the DB
                var artist = new Artist { Name = "Artist 1" };
                var genre = new Genre { Name = "Rock" };
                context.Artists.Add(artist);
                context.Genres.Add(genre);
                context.SaveChanges();

                var controller = new StoreManagerController(context, _logger);
                controller.ModelState.AddModelError("Price", "The value '' is invalid.");

                // Act
                var result = controller.Create(new Album { Title = "Album 1" });

                // Assert
                Assert.IsType<ViewResult>(result);
                var viewResult = result as ViewResult;
                Assert.NotNull(viewResult);
                Assert.NotNull(controller.ViewBag.ArtistId);
                Assert.NotNull(controller.ViewBag.GenreId);
                Assert.Equal(artist.ArtistId.ToString(), ((SelectList)controller.ViewBag.ArtistId).ElementAt(0).Value);
                Assert.Equal(genre.GenreId.ToString(), ((SelectList)controller.ViewBag.GenreId).ElementAt(0).Value);
                Assert.Equal(artist.Name, ((SelectList)controller.ViewBag.ArtistId).ElementAt(0).Text);
                Assert.Equal(genre.Name, ((SelectList)controller.ViewBag.GenreId).ElementAt(0).Text);
                Assert.False(context.Albums.Any(a => a.Title == "Album 1"));
            }
        }

        // Create a test for the Edit method, that accepts an id - valid id should return the view with the model.
        // The controller should populate the ViewBag.ArtistId and ViewBag.GenreId properties
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an album with genre and artist to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Edit method with a valid album id.
        // Assert: Assert that the Edit method returns a ViewResult, along with populating the ViewBag.ArtistId and ViewBag.GenreId properties
        [Fact]
        public void Edit_ValidId_ReturnsViewResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new album & related genre of rock to the DB
                var album = new Album { Title = "Album 1", Artist = new Artist { Name = "Artist 1" }, Genre = new Genre { Name = "Rock" } };
                context.Albums.Add(album);
                context.SaveChanges();

                var controller = new StoreManagerController(context, _logger);

                // Act
                var result = controller.Edit(album.AlbumId);

                // Assert
                Assert.IsType<ViewResult>(result);
                var viewResult = result as ViewResult;
                Assert.NotNull(viewResult);
                Assert.NotNull(viewResult.Model);
                Assert.NotNull(controller.ViewBag.ArtistId);
                Assert.NotNull(controller.ViewBag.GenreId);
                Assert.Equal(album.Title, ((Album)viewResult.Model).Title);
                Assert.Equal(album.Artist.ArtistId, ((SelectList)controller.ViewBag.ArtistId).SelectedValue);
                Assert.Equal(album.Genre.GenreId, ((SelectList)controller.ViewBag.GenreId).SelectedValue);
            }
        }

        // Create a test for the Edit method, that accepts an id - invalid id should return 404.
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an album with genre and artist to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Edit method with an invalid album id.
        // Assert: Assert that the Edit method returns a NotFoundResult
        [Fact]
        public void Edit_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new album & related genre of rock to the DB
                var album = new Album { Title = "Album 1", Artist = new Artist { Name = "Artist 1" }, Genre = new Genre { Name = "Rock" } };
                context.Albums.Add(album);
                context.SaveChanges();

                var controller = new StoreManagerController(context, _logger);

                // Act
                var result = controller.Edit(0);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // Create a test for Edit method, that accepts an id - null id should return 404.
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an album with genre and artist to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Edit method with a null album id.
        // Assert: Assert that the Edit method returns a NotFoundResult
        [Fact]
        public void Edit_NullId_ReturnsNotFoundResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new album & related genre of rock to the DB
                var album = new Album { Title = "Album 1", Artist = new Artist { Name = "Artist 1" }, Genre = new Genre { Name = "Rock" } };
                context.Albums.Add(album);
                context.SaveChanges();

                var controller = new StoreManagerController(context, _logger);

                // Act
                var result = controller.Edit((int?)null);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // Create a test for the Edit method, that accepts an album model - valid model should save to the DB and redirect to Index
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an artist and genre to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Edit method with a valid album model.
        // Assert: Assert that the Edit method returns a RedirectToActionResult, and that the album was updated in the DB
        [Fact]
        public void Edit_ValidModel_RedirectsToIndex()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new artist and genre to the DB
                var artist = new Artist { Name = "Artist 1" };
                var genre = new Genre { Name = "Rock" };
                context.Artists.Add(artist);
                context.Genres.Add(genre);
                context.SaveChanges();

                // Add new album & related genre of rock to the DB
                var album = new Album { Title = "Album 1", ArtistId = artist.ArtistId, GenreId = genre.GenreId, Price = 9.99m };
                context.Albums.Add(album);
                context.SaveChanges();
                context.ChangeTracker.Clear();

                var controller = new StoreManagerController(context, _logger);

                // Act
                var result = controller.Edit(new Album { AlbumId = album.AlbumId, Title = "Album 2", ArtistId = artist.ArtistId, GenreId = genre.GenreId, Price = 9.99m });

                // Assert
                Assert.IsType<RedirectToActionResult>(result);
                var redirectResult = result as RedirectToActionResult;
                Assert.NotNull(redirectResult);
                Assert.Equal("Index", redirectResult.ActionName);
                Assert.Equal("StoreManager", redirectResult.ControllerName);
                Assert.True(context.Albums.Any(a => a.Title == "Album 2"));
            }
        }

        // Create a test for the Edit method, that accepts an album model - invalid model should return the view with the model
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an artist and genre to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Edit method with an invalid album model.
        // Assert: Assert that the Edit method returns a ViewResult, and that the album was not updated in the DB
        [Fact]
        public void Edit_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new artist and genre to the DB
                var artist = new Artist { Name = "Artist 1" };
                var genre = new Genre { Name = "Rock" };
                context.Artists.Add(artist);
                context.Genres.Add(genre);
                context.SaveChanges();

                // Add new album & related genre of rock to the DB
                var album = new Album { Title = "Album 1", ArtistId = artist.ArtistId, GenreId = genre.GenreId, Price = 9.99m };
                context.Albums.Add(album);
                context.SaveChanges();
                context.ChangeTracker.Clear();

                var controller = new StoreManagerController(context, _logger);
                controller.ModelState.AddModelError("Price", "The value '' is invalid.");

                // Act
                var result = controller.Edit(new Album { AlbumId = album.AlbumId, Title = "Album 2", ArtistId = artist.ArtistId, GenreId = genre.GenreId, Price = 9.99m });

                // Assert
                Assert.IsType<ViewResult>(result);
                var viewResult = result as ViewResult;
                Assert.NotNull(viewResult);
                Assert.NotNull(viewResult.Model);
                Assert.NotNull(controller.ViewBag.ArtistId);
                Assert.NotNull(controller.ViewBag.GenreId);
                Assert.Equal("Album 2", ((Album)viewResult.Model).Title);
                Assert.Equal(artist.ArtistId, ((SelectList)controller.ViewBag.ArtistId).SelectedValue);
                Assert.Equal(genre.GenreId, ((SelectList)controller.ViewBag.GenreId).SelectedValue);
            }
        }

        // Create a test for the Delete method, that accepts an id - valid id should remove the album from the DB and redirect to Index
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an album with genre and artist to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Delete method with a valid album id.
        // Assert: Assert that the Delete method returns a RedirectToActionResult, and that the album was removed from the DB
        [Fact]
        public void Delete_ValidId_RedirectsToIndex()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new album & related genre of rock to the DB
                var album = new Album { Title = "Album 1", Artist = new Artist { Name = "Artist 1" }, Genre = new Genre { Name = "Rock" } };
                context.Albums.Add(album);
                context.SaveChanges();

                var controller = new StoreManagerController(context, _logger);

                // Act
                var result = controller.Delete(album.AlbumId);

                // Assert
                Assert.IsType<RedirectToActionResult>(result);
                var redirectResult = result as RedirectToActionResult;
                Assert.NotNull(redirectResult);
                Assert.Equal("Index", redirectResult.ActionName);
                Assert.Equal("StoreManager", redirectResult.ControllerName);
                Assert.False(context.Albums.Any(a => a.Title == "Album 1"));
            }
        }

        // Create a test for the Delete method, that accepts an id - invalid id should return 404.
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an album with genre and artist to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Delete method with an invalid album id.
        // Assert: Assert that the Delete method returns a NotFoundResult
        [Fact]
        public void Delete_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new album & related genre of rock to the DB
                var album = new Album { Title = "Album 1", Artist = new Artist { Name = "Artist 1" }, Genre = new Genre { Name = "Rock" } };
                context.Albums.Add(album);
                context.SaveChanges();

                var controller = new StoreManagerController(context, _logger);

                // Act
                var result = controller.Delete(0);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // Create a test for the Delete method, that accepts an id - null id should return 404.
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add an album with genre and artist to the DB
        // Act: Create a new StoreManagerController using the new MusicStoreContext and the _logger. Call the Delete method with a null album id.
        // Assert: Assert that the Delete method returns a NotFoundResult
        [Fact]
        public void Delete_NullId_ReturnsNotFoundResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new album & related genre of rock to the DB
                var album = new Album { Title = "Album 1", Artist = new Artist { Name = "Artist 1" }, Genre = new Genre { Name = "Rock" } };
                context.Albums.Add(album);
                context.SaveChanges();

                var controller = new StoreManagerController(context, _logger);

                // Act
                var result = controller.Delete((int?)null);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
