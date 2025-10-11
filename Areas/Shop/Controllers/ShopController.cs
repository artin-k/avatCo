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

        // Main shop page with search functionality
        [HttpGet("")]
        public async Task<IActionResult> Index(string? q, string? category, int page = 1)
        {
            var pageSize = 12;
            var productsQuery = _context.Products
                .Where(p => p.IsActive)
                .AsQueryable();

            // Apply search query
            if (!string.IsNullOrWhiteSpace(q))
            {
                productsQuery = productsQuery.Where(p => 
                    p.Title.Contains(q) || 
                    p.Description.Contains(q) ||
                    (p.Brand != null && p.Brand.Contains(q)));
            }

            // Apply category filter
            if (!string.IsNullOrWhiteSpace(category) && category != "all")
            {
                productsQuery = productsQuery.Where(p => p.Category != null && p.Category.Name == category);
            }

            var totalCount = await productsQuery.CountAsync();
            var products = await productsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Category)
                .ToListAsync();

            var model = new ShopViewModel
            {
                ShopName = "avatCo Shop",
                ShopDescription = "Welcome to avatCo Shop - Your one-stop destination for quality products!",
                Products = products,
                Categories = await _context.Categories.ToListAsync()

            };

            // Return partial view for AJAX requests
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductGrid", model);
            }

            return View(model);
        }

        // Quick search for autocomplete
        [HttpGet("QuickSearch")]
        public async Task<IActionResult> QuickSearch(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<string>());

            var suggestions = await _context.Products
                .Where(p => p.Title.Contains(term) && p.IsActive)
                .Select(p => p.Title)
                .Distinct()
                .Take(10)
                .ToListAsync();

            return Json(suggestions);
        }

        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Reviews)
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

        [HttpPost("AddOpinion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOpinion([FromForm] int ProductId, [FromForm] int Rating, [FromForm] string Comment)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null) 
                return Json(new { success = false, message = "Product not found." });

            var review = new Review
            {
                ProductId = ProductId,
                UserName = User.Identity?.Name ?? "Guest",
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