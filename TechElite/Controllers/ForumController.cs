using System.Diagnostics;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using TechElite.Data;
using TechElite.Models;

namespace TechElite.Controllers
{
    public class ForumController : Controller
    {
        public IActionResult Forum()
        {
            return View();
        }

        public IActionResult LatestThreads()
        {
            return View();
        }

        public IActionResult MyThreads()
        {
            return View();
        }

        public IActionResult Thread()
        {
            return View();
        }
    }
}
