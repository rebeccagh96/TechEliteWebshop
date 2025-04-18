﻿using Microsoft.AspNetCore.Mvc;
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
            var users = await _userManager.Users
                .Include(u => u.Customer) 
                .ToListAsync();

            var orders = await _context.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product) 
                .ToListAsync();

            var products = await _context.Products.ToListAsync();
            var departments = await _context.Departments.ToListAsync();
            var customers = await _context.Customers.ToListAsync();
            var userContact = await _context.userContacts.ToListAsync();

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
                    Customer = user.Customer
                });
            }

            var orderViewModels = orders.Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                UserName = order.UserName,
                OrderDate = order.OrderDate,
                OrderProducts = order.OrderProducts.Select(op => new OrderProductViewModel
                {
                    ProductId = op.ProductId,
                    ProductName = op.Product.ProductName,
                    Price = op.Product.Price,
                    ProductQuantity = op.ProductQuantity,
                }).ToList(),
                TotalPrice = order.OrderProducts.Sum(op => op.Product.Price * op.ProductQuantity)
            }).ToList();

            var model = new AdminAccountViewModel
            {
                Users = userViewModels,
                Orders = orderViewModels,
                Customers = customers,
                Products = products.Select(p => new ProductViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Description = p.Description,
                    DepartmentId = p.DepartmentId,
                    DepartmentName = p.Department?.DepartmentName ?? "Unknown"
                }).ToList(),
                Departments = departments,
                UserContacts = userContact
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
            if (!model.ChangePassword)
            {
                ModelState.Remove("Password");
                ModelState.Remove("PasswordConfirm");
                ModelState.Remove("CurrentPassword");
            }

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
            user.UserName = model.UserName ?? user.UserName;
            user.Email = model.Email ?? user.Email;
            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;

            // Uppdatera lösenord om det är angivet
            if (model.ChangePassword && !string.IsNullOrEmpty(model.Password))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    return BadRequest("Nuvarande lösenord krävs för att ändra lösenord.");
                }

                if (model.Password != model.PasswordConfirm)
                {
                    return BadRequest("Lösenorden matchar inte.");
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
                if (!changePasswordResult.Succeeded)
                {
                    return BadRequest("Misslyckades med att uppdatera lösenordet. Kontrollera ditt gamla lösenord.");
                }
            }

            // Uppdatera roller om de är angivna
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

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Misslyckades med att uppdatera användaren");
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
                // Raderar all data relaterad till användaren
                var userReplies = await _context.ForumReplies
                    .Where(r => r.ApplicationUserId == user.Id)
                    .ToListAsync();
                _context.ForumReplies.RemoveRange(userReplies);

                var userThreads = await _context.ForumThreads
                    .Where(t => t.ApplicationUserId == user.Id)
                    .ToListAsync();

                var threadIds = userThreads.Select(t => t.ThreadId).ToList();
                var threadReplies = await _context.ForumReplies
                    .Where(r => threadIds.Contains(r.ThreadId))
                    .ToListAsync();
                _context.ForumReplies.RemoveRange(threadReplies);

                var threadNotifications = await _context.Notifications
                    .Where(n => threadIds.Contains(n.ThreadId))
                    .ToListAsync();
                _context.Notifications.RemoveRange(threadNotifications);

                _context.ForumThreads.RemoveRange(userThreads);

                var userNotifications = await _context.Notifications
                    .Where(n => n.UserId == user.Id)
                    .ToListAsync();
                _context.Notifications.RemoveRange(userNotifications);

                var userReviews = await _context.Reviews
                    .Where(r => r.ApplicationUserId == user.Id)
                    .ToListAsync();
                _context.Reviews.RemoveRange(userReviews);

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
                if (customer != null)
                {
                    var customerOrders = await _context.Orders
                        .Where(o => o.CustomerId == customer.CustomerId)
                        .ToListAsync();
                    _context.Orders.RemoveRange(customerOrders);

                    _context.Customers.Remove(customer);
                }

                await _context.SaveChangesAsync();

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return BadRequest("Misslyckades med att hämta användaren från Identity.");
                }

                await transaction.CommitAsync();
                return RedirectToAction(nameof(HomeController.Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { success = false, message = "Ett fel inträffade vid borttagning: " + ex.Message });
            }
        }

    }
}