using avatCo.Models;
using avatCo.Models.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BCrypt.Net;


namespace avatCo.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Route("Profile")]
    public class DashboardController : Controller
    {
        private readonly AvatDbContext _context;

        public DashboardController(AvatDbContext context)
        {
            _context = context;
        }


        [HttpGet("")]   //  This makes /Profile map here
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
/*            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password) // ✅ hash before save
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, redirectUrl = "/Profile" });
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
 //           await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Profile", new { area = "UserProfile" });
        }


    }
}
