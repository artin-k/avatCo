using avatCo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace avatCo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AvatDbContext _context;

        public ProductsController(AvatDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            var products = _context.Products.Include(p => p.Category).ToList();

            ViewBag.Categories = categories;
            return View(products); // assuming your model is still List<Product>
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Product model, IFormFile? ImageFile)
        {
            try
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");
                    Directory.CreateDirectory(folder);
                    var filename = Path.GetFileName(ImageFile.FileName);
                    var path = Path.Combine(folder, filename);
                    using var stream = new FileStream(path, FileMode.Create);
                    await ImageFile.CopyToAsync(stream);
                    model.ImageUrl = "/uploads/products/" + filename;
                }

                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                return Json(new { success = true, id = model.Id });
            }
            catch (Exception ex)
            {
                // log ex
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            return Json(new
            {
                id = product.Id,
                isActive = product.IsActive,
                isSpecialOffer = product.IsSpecialOffer,
                title = product.Title,
                price = product.Price,
                description = product.Description,
                categoryId = product.CategoryId,
                imageUrl = string.IsNullOrEmpty(product.ImageUrl) ? null : product.ImageUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] Product model, IFormFile? ImageFile)
        {
            try
            {
                if (model.Id == 0)
                    return Json(new { success = false, message = "Id missing in form data" });

                var existing = await _context.Products.FindAsync(model.Id);
                if (existing == null)
                    return Json(new { success = false, message = $"Product not found with Id={model.Id}" });

                existing.Title = model.Title;
                existing.Price = model.Price;
                existing.Description = model.Description;
                existing.CategoryId = model.CategoryId;
                existing.IsActive = model.IsActive;
                existing.IsSpecialOffer = model.IsSpecialOffer;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");
                    Directory.CreateDirectory(folder);
                    var filename = Path.GetFileName(ImageFile.FileName);
                    var path = Path.Combine(folder, filename);
                    using var stream = new FileStream(path, FileMode.Create);
                    await ImageFile.CopyToAsync(stream);
                    existing.ImageUrl = "/uploads/products/" + filename;
                }

                _context.Products.Update(existing);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null) return Json(new { success = false, message = "Not found" });

            _context.Products.Remove(existing);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
