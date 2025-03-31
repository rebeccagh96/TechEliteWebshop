using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TechElite.Data;
using TechElite.Models;

namespace TechElite.Controllers
{
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult Forum()
        {
            return View();
        }

        public IActionResult LatestThreads()
        {
            if (_context is not null)
            {
                for (int i = 0; i < _context.ForumThreads.Count(); i++)
                {
                    return View(i);
                }
            }
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
