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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product products)
        {
            /*            if (ModelState.IsValid)
                        {*/

            var existing = await _context.Products.FindAsync(products.Id);
            if (existing != null)
            {

                existing.Title = products.Title;
                existing.Description = products.Description;

                if (products.ImageFile != null && products.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(products.ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Products", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await products.ImageFile.CopyToAsync(stream);
                    }

                    existing.ImageUrl = "/uploads/Products/" + fileName;
                }

                _context.Update(existing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();

            /*            }*/
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
