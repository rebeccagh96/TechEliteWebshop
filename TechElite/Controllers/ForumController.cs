using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TechElite.Data;
using TechElite.Models;

namespace TechElite.Controllers
{
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public ForumController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            ForumViewModel model = new (
                ForumCategories: _context.ForumCategories.ToList(),
                ForumThreads: _context.ForumThreads.ToList(),
                Replies: _context.Replies.ToList()
                );
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddThread(ForumThread forumThread)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                forumThread.UserId = userId;
                if (userId == null)
                {
                    return View();
                }

                var publishDate = DateTime.Now;
                forumThread.PublishDate = publishDate;

                _context.Add(forumThread);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(); 
            }
            
        }

        public IActionResult LatestThreads()
        {
            ForumViewModel model = new(
            ForumCategories: _context.ForumCategories.ToList(),
            ForumThreads: _context.ForumThreads.ToList(),
            Replies: _context.Replies.ToList()
            );
            return View(model);
        }

        /*[HttpGet]
        public async Task<IActionResult> ShowLatestThreads()
        {
            var forumThreads = await _context.ForumThreads.ToListAsync();
            return View(forumThreads);
        }*/

        /*[HttpGet]
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
        }*/


        public IActionResult UpdateThread(ForumThread forumThread)
        {
                _context.ForumThreads.Update(forumThread);
                _context.SaveChanges();

            return View();
        }

        public IActionResult DeleteThread(int id)
        {
            using (var context = _context)
            {
                var thread = context.ForumThreads.SingleOrDefault(t => t.ForumThreadId == id);
                if (thread != null)
                {
                    context.ForumThreads.Remove(thread);
                    context.SaveChanges();
                }
            }
            return View();
        }

        public IActionResult MyThreads()
        {
            return View();
        }

        public IActionResult Thread(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("You must pass in a ThreadId.");
            }
            
            return View();
        }
    }
}
