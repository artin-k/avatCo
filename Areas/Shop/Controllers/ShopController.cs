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
        public async Task<IActionResult> Index()
        {
            var model = new ShopViewModel()
            {
                ShopName = "",
                ShopDescription = "",
                Products = await _context.Products.ToListAsync(),
                Categories = await _context.Categories.ToListAsync()
            };
            return View(model);
        }
    }
}