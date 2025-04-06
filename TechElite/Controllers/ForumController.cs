using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TechElite.Areas.Identity.Data;
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
                ForumReplies: _context.ForumReplies.ToList()
                );
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.ForumCategories.ToListAsync();
            if (categories == null || !categories.Any())
            {
                Console.WriteLine("Inga kategorier hittades.");
            }
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int categoryId, string threadTitle, string threadContent)
        {
            List<ForumThread> forumThreads = await _context.ForumThreads.ToListAsync();
            var forumThread = new ForumThread
            {
                CategoryId = categoryId,
                ThreadTitle = threadTitle,
                ThreadContent = threadContent,
                ApplicationUserId = "USER1-STATIC-ID",
                UserName = "user1",
                PublishDate = DateTime.Now
            };
            _context.ForumThreads.Add(forumThread);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult LatestThreads()
        {
            ForumViewModel model = new(
            ForumCategories: _context.ForumCategories.ToList(),
            ForumThreads: _context.ForumThreads.ToList(),
            ForumReplies: _context.ForumReplies.ToList()
            );
            return View(model);
        }

        [HttpGet]
        public IActionResult MyThreads()
        {
            string? id = User.Identity?.Name;
            if (id is null)
            {
                return BadRequest("You must be logged in.");
            }
            var forumThreads = _context.ForumThreads
                .Where(ft => ft.ApplicationUserId == id)
                .ToList();
            return View(forumThreads);
        }

        [HttpGet]
        public IActionResult Thread(int? id)
        {
            if (id is null)
            {
                return BadRequest("You must pass in a ThreadId.");
            }
            var model = _context.ForumThreads
                .Include(t => t.Replies)
                .ThenInclude(r => r.ApplicationUser)
                .FirstOrDefault(t => t.ThreadId == id);
            if (model == null)
            {
                return NotFound($"Forum thread with ID {id} was not found.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReply(int threadId, string replyContent, string threadTitle, string threadContent, int categoryId)
        {
            ForumThread? model = _context.ForumThreads
                .SingleOrDefault(t => t.ThreadId == threadId);
            List<ForumReply> forumReplies = await _context.ForumReplies.ToListAsync();
            var reply = new ForumReply
            {
                ThreadId = threadId,
                ApplicationUserId = "USER1-STATIC-ID",
                PublishDate = DateTime.Now,
                Content = replyContent,
                Thread = model
            };

            _context.ForumReplies.Add(reply);
            await _context.SaveChangesAsync();
            return RedirectToAction("Thread", new { threadId = model.ThreadId });
        }

        [HttpGet]
        public IActionResult UpdateThread(int? id)
        {
            if (id == null)
            {
                return BadRequest("You must pass in a ThreadId.");
            }
            var model = _context.ForumThreads
                .Include(t => t.Replies)
                .ThenInclude(r => r.ApplicationUser)
                .FirstOrDefault(t => t.ThreadId == id);
            if (model == null)
            {
                return NotFound($"Forum thread with ID {id} was not found.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateThread(int id, int categoryId, string threadTitle, string threadContent)
        {
            var forumThread = await _context.ForumThreads
                .FirstOrDefaultAsync(t => t.ThreadId == id);
            if (forumThread == null)
            {
                return NotFound("Thread not found.");
            }
            forumThread.ThreadTitle = threadTitle;
            forumThread.ThreadContent = threadContent;
            forumThread.CategoryId = categoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction("Thread", new { id = forumThread.ThreadId });
        }

        public async Task<IActionResult> DeleteThread(int? id)
        {
            var thread = await _context.ForumThreads
                .Include(t => t.Replies)
                .FirstOrDefaultAsync(t => t.ThreadId == id);
            if (thread == null)
            {
                return NotFound("Thread not found");
            }
            if (thread != null)
            {
                _context.ForumReplies.RemoveRange(thread.Replies);
                _context.ForumThreads.Remove(thread);
                await _context.SaveChangesAsync();
            }
            return View();
        }

    }
}
