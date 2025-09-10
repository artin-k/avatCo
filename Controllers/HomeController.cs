using avatCo.Models;
using avatCo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace avatCo.Controllers
{
    public class HomeController : Controller
    {
        private readonly AvatDbContext _context;

        public HomeController(AvatDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var model = new HomePageViewModel
            {
                HeroTitle = "Welcome to Avat Co",
                HeroSubtitle = "Scalable solutions for modern businesses",
                FeaturedProducts = _context.Products
                                            .Where(p => p.IsActive)
                                            .Take(4)
                                            .ToList()
            };

            return View(model); // ✅ Pass the model to the view
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
