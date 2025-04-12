using Microsoft.AspNetCore.Mvc;

namespace TechElite.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
