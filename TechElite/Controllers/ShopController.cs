using Microsoft.AspNetCore.Mvc;

namespace TechElite.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
