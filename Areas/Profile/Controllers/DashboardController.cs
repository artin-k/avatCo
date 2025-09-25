using avatCo.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace avatCo.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Route("Profile")]
    public class DashboardController : Controller
    {
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // TODO: Save user to DB
            // Hash password, validate, etc.

            TempData["Success"] = "ثبت نام با موفقیت انجام شد!";
            return RedirectToAction("Index", "Dashboard", new { area = "UserProfile" });
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
