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

            var model = new ShopViewModel
            {
                ShopName = "avatCo Shop",
                ShopDescription = "Welcome to avatCo Shop - Your one-stop destination for quality products!",
                Products = await query.ToListAsync(),
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

        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Reviews) // make sure you named it Reviews in Product model
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var result = new
            {
                id = product.Id,
                title = product.Title ?? "",
                description = product.Description ?? "",
                imageUrl = string.IsNullOrEmpty(product.ImageUrl) ? Url.Content("~/images/placeholder.png") : product.ImageUrl,
                price = product.Price,
                isActive = product.IsActive,
                brand = product.Brand ?? "",
                material = product.Material ?? "",
                categoryName = product.Category?.Name ?? "",
                opinions = product.Reviews?
                .Select(r => new Review
                {
                    UserName = r.UserName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                }).ToList() ?? new List<Review>()
            };

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOpinion([FromForm] int ProductId, [FromForm] int Rating, [FromForm] string Comment)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null) return Json(new { success = false, message = "Product not found." });

            var review = new Review
            {
                ProductId = ProductId,
                UserName = "Guest", // replace with current user if auth exists
                Rating = Rating,
                Comment = Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Json(new { success = true, productId = ProductId });
        }

    }

}

