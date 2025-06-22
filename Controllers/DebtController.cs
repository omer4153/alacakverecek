using alacakverecektakip.Data;
using alacakverecektakip.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace alacakverecektakip.Controllers
{
    [Authorize]
    public class DebtController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DebtController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var debts = await _context.Debts
                .Include(d => d.Customer)
                .Where(d => d.Customer.UserId == userId)
                .ToListAsync();
            return View(debts);
        }

        public IActionResult Create()
        {
            ViewBag.Customers = _context.Customers
                .Where(c => c.UserId == _userManager.GetUserId(User))
                .ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,Amount,Type,Description,TransactionDate")] Debt debt)
        {
            if (ModelState.IsValid)
            {
                debt.CreatedAt = DateTime.Now;
                _context.Add(debt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var userId = _userManager.GetUserId(User);
            ViewData["CustomerId"] = new SelectList(_context.Customers.Where(c => c.UserId == userId), "Id", "Name", debt.CustomerId);
            return View(debt);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var debt = await _context.Debts.FindAsync(id);
            if (debt == null) return NotFound();

            return View(debt);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var debt = await _context.Debts.FindAsync(id);
            _context.Debts.Remove(debt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
