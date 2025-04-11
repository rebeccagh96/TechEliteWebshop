//using System.Diagnostics;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechElite.Areas.Identity.Data;
using TechElite.Models;
using TechElite.Areas.Identity.Pages;
using Microsoft.AspNetCore.Identity;

namespace TechElite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.ApplicationUserId == currentUserId);
            var users = await _userManager.Users.ToListAsync();
            var orders = await _context.Orders.Include(o => o.Products).ToListAsync();
            var products = await _context.Products.ToListAsync();
            var departments = await _context.Departments.ToListAsync();
            var customers = await _context.Customers.ToListAsync();


            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = roles.ToList(),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                });
            }

            var model = new AdminAccountViewModel
            {
                Users = userViewModels,
                Orders = orders,
                //Customers = customers.Select(c => new Customer
                //{
                //    CustomerId = c.CustomerId,
                //    FirstName = c.FirstName,
                //    LastName = c.LastName,
                //    Address = c.Address,
                //    City = c.City,
                //    ZipCode = c.ZipCode
                //}).ToList(),
                Customers = customers,
                Products = products.Select(p => new ProductViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Description = p.Description,
                    DepartmentId = p.DepartmentId,
                    DepartmentName = p.Department?.DepartmentName ?? "Unknown",
                    Image = p.Image
                }).ToList(),
                Departments = departments
            };

            return View(model);
        }



        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Användar ID krävs.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Användare kunde inte hittas");
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,

            };

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Ogiltig data", errors });
            }

            // Hämta användaren baserat på ID
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound("Användaren hittades inte.");
            }

            // Uppdatera ApplicationUser-egenskaper
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Misslyckades med att uppdatera användaren");
            }

            // Updaterar roll
            if (model.Roles != null && model.Roles.Any())
            {
                var selectedRole = model.Roles.First(); // Hämtar rollen
                var currentRoles = await _userManager.GetRolesAsync(user);

                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeRolesResult.Succeeded)
                {
                    return BadRequest("Misslyckades med att ta bort nuvarande roll.");
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, selectedRole);
                if (!addRoleResult.Succeeded)
                {
                    return BadRequest("Misslyckades med att lägga till vald roll");
                }
            }

            return Ok(new { success = true, message = "Användaruppdatering lyckades." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteUserRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                return BadRequest("Användar ID krävs.");
            }

            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return NotFound("Användare hittades inte.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var userId = user.Id;

                // Raderar reviews relaterade till användaren osv för kommande egenskaper
                var reviews = await _context.Reviews
                    .Where(r => r.ApplicationUserId == userId)
                    .ToListAsync();
                _context.Reviews.RemoveRange(reviews);

                var replies = await _context.ForumReplies
                    .Where(r => r.ApplicationUserId == userId)
                    .ToListAsync();
                _context.ForumReplies.RemoveRange(replies);

                var threads = await _context.ForumThreads
                    .Where(t => t.ApplicationUserId == userId)
                    .ToListAsync();
                _context.ForumThreads.RemoveRange(threads);

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return BadRequest("Misslyckades med att hämta användaren från Identity.");
                }

                //  Sparar alla raderingar
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { success = true, message = "Användaren raderades framgångsrikt." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { success = false, message = "Ett fel inträffade vid borttagning: " + ex.Message });
            }
        }

    }
}