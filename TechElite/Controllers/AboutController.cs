using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TechElite.Data;
using TechElite.Models;

namespace TechElite.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
