using Microsoft.AspNetCore.Mvc;

namespace avatCo.Areas.Shop
{
    [Area("Shop")]
    [Route("Shop")]
    public class ShopController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}