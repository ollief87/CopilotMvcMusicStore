using CopilotMvcMusicStore.Web.Controllers;
using CopilotMvcMusicStore.Web.Data;
using CopilotMvcMusicStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CopilotMvcMusicStore.Test
{
    public class StoreControllerTests
    {
        // Mock the ILogger<StoreController> and MusicStoreContext
        private readonly ILogger<StoreController> _logger;
        private readonly DbContextOptions<MusicStoreContext> _options;

       

        public StoreControllerTests()
        {
            // Mock the ILogger<StoreController> using NSubstitute
            _logger = Substitute.For<ILogger<StoreController>>();
            _options = new DbContextOptionsBuilder<MusicStoreContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        // Create a test for the Index method
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable
        // Act: Create a new StoreController using the new MusicStoreContext and the _logger
        // Assert: Assert that the Index method returns a ViewResult
        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                var controller = new StoreController(context, _logger);

                // Act
                var result = controller.Index();

                // Assert
                Assert.IsType<ViewResult>(result);
            }
        }

        // Create a test for the Browse method
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable
        // Act: Create a new StoreController using the new MusicStoreContext and the _logger
        // Assert: Assert that the Browse method returns a ViewResult
        [Fact]
        public void Browse_ReturnsViewResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new album & related genre of rock to the DB
                var album = new Album { Title = "Test Album", Price = 9.99m, Genre = new Genre { Name = "Rock" } };
                context.Albums.Add(album);
                context.SaveChanges();

                var controller = new StoreController(context, _logger);

                // Act
                var result = controller.Browse("Rock");

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal("Rock", ((Genre)((ViewResult)result).Model).Name);
                Assert.True(((Genre)((ViewResult)result).Model).Albums.Count == 1);
            }
        }

        // Create a test for the Details method
        // The test should use Arrange, Act Assert methodology
        // Arrange: Create a new MusicStoreContext using the _options variable, add a single album to the DB along with a genre and artist
        // Act: Create a new StoreController using the new MusicStoreContext and the _logger
        // Assert: Assert that the Details method returns a ViewResult, as well as the correct album, artist & genre
        [Fact]
        public void Details_ReturnsViewResult()
        {
            // Arrange
            using (var context = new MusicStoreContext(_options))
            {
                // Add new album & related genre of rock to the DB
                var album = new Album { Title = "Test Album", Price = 9.99m, Genre = new Genre { Name = "Rock" }, Artist = new Artist { Name = "Test Artist" } };
                context.Albums.Add(album);
                context.SaveChanges();

                var controller = new StoreController(context, _logger);

                // Act
                var result = controller.Details(album.AlbumId);

                // Assert
                Assert.IsType<ViewResult>(result);
                Assert.Equal("Test Album", ((Album)((ViewResult)result).Model).Title);
                Assert.Equal("Test Artist", ((Album)((ViewResult)result).Model).Artist.Name);
                Assert.Equal("Rock", ((Album)((ViewResult)result).Model).Genre.Name);
            }
        }
        
    }
}