using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TechElite.Areas.Identity.Data;
using TechElite.Models;

namespace TechElite.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
