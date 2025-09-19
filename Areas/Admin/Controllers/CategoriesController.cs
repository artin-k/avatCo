using Microsoft.AspNetCore.Mvc;
using avatCo.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace avatCo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly AvatDbContext _context;
        public CategoriesController(AvatDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var cats = await _context.Categories.ToListAsync();
            return View(cats);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ImageFile != null)
                {
                    var fileName = Path.GetFileName(category.ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Categories", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await category.ImageFile.CopyToAsync(stream);
                    }

                    // Save path to DB
                    category.ImageUrl = "/uploads/Categories/" + fileName;
                }

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return Json(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
/*            if (ModelState.IsValid)
            {*/

                var existing = await _context.Categories.FindAsync(category.Id);
                if (existing != null)
                {

                    existing.Name = category.Name;
                    existing.Description = category.Description;

                    if (category.ImageFile != null)
                    {
                        var fileName = Path.GetFileName(category.ImageFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Categories", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await category.ImageFile.CopyToAsync(stream);
                        }

                        existing.ImageUrl = "/uploads/Categories/" + fileName;
                    }

                    _context.Update(existing);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();

/*            }*/
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return Json(new { success = false, message = "Category not found" });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
