using avatCo.Models;
using avatCo.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace avatCo.Areas.Shop
{
    [Area("Shop")]
    [Route("Shop")]
    public class ShopController : Controller
    {
        private readonly AvatDbContext _context;
        public ShopController(AvatDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string? q)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p => p.Title.Contains(q) || p.Description.Contains(q));
            }
            var model = new ProductDetailsViewModel()
            {
                Title = "",
                Description = "",
                IsActive = await _context.Products.Where(p => p.IsActive).AnyAsync(),
                Price = await _context.Products.Where(p => p.IsActive).Select(p => p.Price).FirstOrDefaultAsync(),
                Opinions = await _context.Reviews.ToListAsync(),
                Products = await _context.Products.ToListAsync(),
                Categories = await _context.Categories.ToListAsync()
            };
            return View(model);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var results = await _context.Products
                .Where(p => p.Title.Contains(term) || p.Description.Contains(term))
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.ImageUrl
                })
                .Take(5) // limit results
                .ToListAsync();

            return Json(results);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Reviews)

                .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

    }
}