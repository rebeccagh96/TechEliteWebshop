using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TechElite.Models;

namespace TechElite.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
