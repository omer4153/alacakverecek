using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using alacakverecektakip.Models;
using alacakverecektakip.ViewModels;
using alacakverecektakip.Data;
using System.Threading.Tasks;
using System.Linq;

namespace alacakverecektakip.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Otomatik giriş
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Eğer kullanıcı için daha önce Customer eklenmemişse, ekle
                    var existingCustomer = _context.Customers.FirstOrDefault(c => c.UserId == user.Id);
                    if (existingCustomer == null)
                    {
                        var customer = new Customer
                        {
                            UserId = user.Id,
                            Name = model.Email,
                            Email = model.Email,
                            CreatedAt = DateTime.Now
                        };

                        _context.Customers.Add(customer);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("Index", "Home");
                }

                // Hataları göster
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    await _signInManager.SignInAsync(user, model.RememberMe);

                    // Customer kaydı yoksa oluştur
                    var existingCustomer = _context.Customers.FirstOrDefault(c => c.UserId == user.Id);
                    if (existingCustomer == null)
                    {
                        var customer = new Customer
                        {
                            UserId = user.Id,
                            Name = user.UserName ?? "İsimsiz",
                            Email = user.Email,
                            CreatedAt = DateTime.Now
                        };

                        _context.Customers.Add(customer);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Geçersiz giriş.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
