using avatCo.Models;
using avatCo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace avatCo.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : Controller
    {
        private readonly AvatDbContext _context;

        public HomeController(AvatDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var model = new HomePageViewModel
            {
                HeroTitle = "Welcome to Avat Co",
                HeroSubtitle = "Scalable solutions for modern businesses",
                FeaturedProducts = await _context.Products.ToListAsync(),
                Categories = await _context.Categories.ToListAsync()

            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
