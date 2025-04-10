using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
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


        public async Task<IActionResult> Index()
        {
            var forumCategories = await _context.ForumCategories.ToListAsync();
            var forumThreads = await _context.ForumThreads.ToListAsync();
            var forumReplies = await _context.ForumReplies.ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            List<Notification> notification = new();

            if (user != null)
            {
                notification = await _context.Notifications
                    .Where(n => n.UserId == user.Id)
                    .ToListAsync();
            }


            ForumViewModel model = new (
                ForumCategories: forumCategories,
                ForumThreads: forumThreads,
                ForumReplies: forumReplies,
                CurrentUser: user,
                Notifications: notification
                );
                
            return View(model);
        }

            [HttpGet]
        public async Task<IActionResult> Create()
        {
            var forumCategories = await _context.ForumCategories.ToListAsync();
            var forumThreads = await _context.ForumThreads.ToListAsync();
            var forumReplies = await _context.ForumReplies.ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            var notification = await _context.Notifications.ToListAsync();

            if (user is null)
            {
                return BadRequest("You must be logged in.");
            }

            ForumViewModel model = new(
                ForumCategories: forumCategories,
                ForumThreads: forumThreads,
                ForumReplies: forumReplies,
                CurrentUser: user,
                Notifications: notification
                );

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int categoryId, string threadTitle, string threadContent)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest("You must be logged in.");
            }
            List<ForumThread> forumThreads = await _context.ForumThreads.ToListAsync();
            var forumThread = new ForumThread
            {
                CategoryId = categoryId,
                ThreadTitle = threadTitle,
                ThreadContent = threadContent,
                ApplicationUserId = user.Id,
                UserName = user.UserName,
                PublishDate = DateTime.Now
            };
            _context.ForumThreads.Add(forumThread);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MyThreads()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                return BadRequest("You must be logged in.");
            }
            var forumThreads = await _context.ForumThreads
                .Where(ft => ft.ApplicationUserId == user.Id)
                .ToListAsync();
            return View(forumThreads);
        }

        [HttpGet]
        public async Task<IActionResult> Thread(int? id)
        {
            if (id is null)
            {
                return BadRequest("You must pass in a ThreadId.");
            }
            var model = await _context.ForumThreads
                .Include(t => t.Replies)
                .ThenInclude(r => r.ApplicationUser)
                .FirstOrDefaultAsync(t => t.ThreadId == id);
            if (model == null)
            {
                return NotFound($"Forum thread with ID {id} was not found.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReply(int threadId, string replyContent, string threadTitle, string threadContent, int categoryId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest("You must be logged in.");
            }
            ForumThread? model = await _context.ForumThreads
                .Include(t => t.ApplicationUser)
                .SingleOrDefaultAsync(t => t.ThreadId == threadId);
            if (model == null)
            {
                return NotFound("Thread not found.");
            }

            var reply = new ForumReply
            {
                ThreadId = threadId,
                ApplicationUserId = user.Id,
                UserName = user.UserName,
                PublishDate = DateTime.Now,
                Content = replyContent,
                Thread = model
            };

            _context.ForumReplies.Add(reply);
            await _context.SaveChangesAsync();

            if(model.ApplicationUserId != user.Id)
            {
                var notification = new Notification
                {
                    UserId = model.ApplicationUserId,
                    Message = $"Nytt inlägg i din tråd: ",
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    ThreadId = threadId
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Thread", new { id = model.ThreadId });
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
        public async Task<IActionResult> UpdateThread(int id, string threadTitle, string threadContent)
        {
            var forumThread = await _context.ForumThreads
                .FirstOrDefaultAsync(t => t.ThreadId == id);
            if (forumThread == null)
            {
                return NotFound("Thread not found.");
            }
            forumThread.ThreadTitle = threadTitle;
            forumThread.ThreadContent = threadContent;

            await _context.SaveChangesAsync();
            return RedirectToAction("Thread", new { id = forumThread.ThreadId });
        }

        [HttpGet]
        public IActionResult UpdateReply(int? id)
        {
            if (id == null)
            {
                return BadRequest("You must pass in a ReplyId.");
            }
            var model = _context.ForumReplies
                .Include(r => r.ApplicationUser)
                .FirstOrDefault(r => r.ReplyId == id);
            if (model == null)
            {
                return NotFound($"Reply with ID {id} was not found.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReply(int id, string replyContent)
        {
            var forumReply = await _context.ForumReplies
                .FirstOrDefaultAsync(r => r.ReplyId == id);
            if (forumReply == null)
            {
                return NotFound("Reply not found.");
            }
            forumReply.Content = replyContent;

            await _context.SaveChangesAsync();
            return RedirectToAction("Thread", new { id = forumReply.ThreadId });
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
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteReply(int? id) 
        {
            var reply = await _context.ForumReplies
                .FirstOrDefaultAsync(r => r.ReplyId == id);
            if (reply == null)
            {
                return NotFound("Reply not found");
            }
            if (reply != null)
            {
                _context.ForumReplies.Remove(reply);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Thread", new { id = reply.ThreadId});
        }

        public async Task<IActionResult> LatestThreads()
        {
            var forumCategories = await _context.ForumCategories.ToListAsync();
            var forumThreads = await _context.ForumThreads.ToListAsync();
            var forumReplies = await _context.ForumReplies.ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            var notification = await _context.Notifications.ToListAsync();

            ForumViewModel model = new(
                ForumCategories: forumCategories,
                ForumThreads: forumThreads,
                ForumReplies: forumReplies,
                CurrentUser: user,
                Notifications: notification
                );
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FilterThreads(int? id)
        {
            if (id is null)
            {
                return BadRequest("You must pass in a CategoryId.");
            }

            var model = await _context.ForumThreads
                .Where(c => c.CategoryId == id)
                .ToListAsync();
            if (model == null)
            {
                return NotFound($"Forum category with ID {id} was not found.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchForum(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return BadRequest("Hittade inget sökord.");
            }

            var categories = await _context.ForumCategories.ToListAsync();
            var forumThreads = await _context.ForumThreads.ToListAsync();
            var replies = await _context.ForumReplies.ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            var notification = await _context.Notifications.ToListAsync();

            var filteredThreads = forumThreads.
                Where(t => t.ThreadTitle.Contains(search, StringComparison.OrdinalIgnoreCase) 
                || t.ThreadContent.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var filteredReplies = replies.
                Where(r => r.Content.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var forumViewModel = new ForumViewModel(
                ForumCategories: categories,
                ForumThreads: filteredThreads,
                ForumReplies: filteredReplies,
                CurrentUser: user,
                Notifications: notification
                );
            return View(forumViewModel);
        }

        public async Task<IActionResult> Read(int id)
        {
            var notification = await _context.Notifications
                .Include(n => n.ForumThread)
                .FirstOrDefaultAsync(n => n.NotificationId == id);

            if (notification == null)
            {
                return NotFound();
            }
            if (!notification.IsRead)
            {
                notification.IsRead = true;
                _context.Update(notification);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Thread", new { id = notification.ThreadId });
        }
    }
}
