using CopilotMvcMusicStore.Web.Data;
using CopilotMvcMusicStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CopilotMvcMusicStore.Web.Controllers
{
    public class StoreManagerController : Controller
    {
        // DB Context
        private readonly MusicStoreContext _context;
        private readonly ILogger<StoreManagerController> _logger;

        public StoreManagerController(MusicStoreContext context, ILogger<StoreManagerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Index method that returns a view with all Albums in the DB, including Artist & Genre
        public ActionResult Index()
        {
            var albums = _context.Albums.Include("Artist").Include("Genre").ToList();
            return View(albums);
        }

        // Create method returns empty view, the method populates ViewBag.ArtistId and ViewBag.GenreId
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(_context.Artists, "ArtistId", "Name");
            ViewBag.GenreId = new SelectList(_context.Genres, "GenreId", "Name");
            return View();
        }

        // Create method that accepts an album model and adds it to the DB
        // If the model is valid, write Album to DB & redirect to Index
        // Otherwise populate ViewBag.ArtistId and ViewBag.GenreId based on Album argument passed to the method
        // Return view and the Album
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Albums.Add(album);
                _context.SaveChanges();
                return RedirectToAction("Index", "StoreManager");
            }

            ViewBag.ArtistId = new SelectList(_context.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(_context.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // Edit method that accepts an id, the method populates ViewBag.ArtistId and ViewBag.GenreId
        // If the id is null, return 404
        // Otherwise retrieve the album from the DB, if it doesn't exist return 404
        // Return view and the Album
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = _context.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }

            ViewBag.ArtistId = new SelectList(_context.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(_context.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // Edit method that accepts an album model and updates it in the DB
        // If the model is valid, update Album in DB & redirect to Index
        // Otherwise populate ViewBag.ArtistId and ViewBag.GenreId based on Album argument passed to the method
        // Return view and the Album
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(album).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index", "StoreManager");
            }

            ViewBag.ArtistId = new SelectList(_context.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(_context.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // Delete method that accepts an id
        // If the id is null, return 404
        // Otherwise retrieve the album from the DB, if it doesn't exist return 404
        // Delete the album from the DB and redirect to Index
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = _context.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }

            _context.Albums.Remove(album);
            _context.SaveChanges();
            return RedirectToAction("Index", "StoreManager");
        }

    }
}
