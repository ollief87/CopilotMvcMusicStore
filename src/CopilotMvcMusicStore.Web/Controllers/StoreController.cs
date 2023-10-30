using CopilotMvcMusicStore.Web.Models;
using CopilotMvcMusicStore.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace CopilotMvcMusicStore.Web.Controllers
{
    public class StoreController : Controller
    {
        // add dbcontext to storecontroller
        private readonly MusicStoreContext _context;
        private readonly ILogger<StoreController> _logger;

        // constructor with logger
        public StoreController(MusicStoreContext context, ILogger<StoreController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // add three mothods named Index Browse Details
        public ActionResult Index()
        {
            // get list of genres from db
            var genres = _context.Genres.ToList();
            return View(genres);
        }

        public ActionResult Browse(string genre)
        {
            // sanitize genre input
            genre = HttpUtility.HtmlEncode(genre);

            // retrieve genre and its associated albums from db
            var genreModel = _context.Genres.Include("Albums").Single(g => g.Name == genre);
            return View(genreModel);
        }

        // details method that returns a view with an album model
        public ActionResult Details(int id)
        {
            // retrieve the album from the database include artist and genre
            var album = _context.Albums.Include("Artist").Include("Genre").Single(a => a.AlbumId == id);
            return View(album);
        }
    }
}
