using Microsoft.AspNetCore.Mvc;

namespace alacakverecektakip.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}