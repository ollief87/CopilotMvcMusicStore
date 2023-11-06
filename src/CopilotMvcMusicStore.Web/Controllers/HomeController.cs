using CopilotMvcMusicStore.Web.Data;
using CopilotMvcMusicStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CopilotMvcMusicStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicStoreContext _dbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(MusicStoreContext musicStoreContext, ILogger<HomeController> logger)
        {
            _dbContext = musicStoreContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Return the MusicStoreSummary view, passing in a list of MusicStoreSummary objects
            return View(_dbContext.MusicStoreSummaries.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}