using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using alacakverecektakip.Data;
using alacakverecektakip.Models;

namespace alacakverecektakip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Customers()
        {
            var customers = await _context.Customers
                .Include(c => c.UserId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(customers);
        }

        public async Task<IActionResult> Debts()
        {
            var debts = await _context.Debts
                .Include(d => d.Customer)
                .ThenInclude(c => c.UserId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
            return View(debts);
        }
    }
}
