using avatCo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace avatCo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]/")]
    public class ProductsController : Controller
    {
        private readonly AvatDbContext _context;

        public ProductsController(AvatDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
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
        public IActionResult Create(Product model, IFormFile ImageFile)
        {
            Console.WriteLine("create product hit");

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine("wwwroot/uploads/products", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                model.ImageUrl = "/uploads/products/" + fileName; // Assuming your Product model has ImagePath
            }
            _context.Products.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
        {
            Console.WriteLine("✏️ Edit product hit");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("❌ Model error: " + error.ErrorMessage);
                }
                return View(product);
            }

            var dbProduct = await _context.Products.FindAsync(product.Id);
            if (dbProduct == null)
            {
                Console.WriteLine("❌ Product not found");
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                dbProduct.ImageUrl = "/uploads/products/" + fileName;
            }

            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.Price = product.Price;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
