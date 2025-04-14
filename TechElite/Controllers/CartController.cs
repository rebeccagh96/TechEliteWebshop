using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using TechElite.Models;
using TechElite.Areas.Identity.Data;
using TechElite.Helpers;

namespace TechElite.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ViewCart()
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<OrderProductViewModel>>("Cart") ?? new List<OrderProductViewModel>();
            var model = new CartPageViewModel
            {
                CartItems = cartItems,
                Customer = new Customer
                {
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Address = string.Empty,
                    ZipCode = string.Empty,
                    City = string.Empty,
                    UserName = User.Identity?.Name ?? "Guest"
                }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ViewCart", model); // Show validation messages
            }

            var customer = new Customer
            {
                FirstName = model.Customer.FirstName,
                LastName = model.Customer.LastName,
                Address = model.Customer.Address,
                ZipCode = model.Customer.ZipCode,
                City = model.Customer.City,
                ApplicationUserId = model.Customer.ApplicationUserId,
                UserName = model.Customer.UserName
            };

            var order = new Order
            {
                OrderDate = DateTime.Now,
                UserName = model.Customer.UserName ?? "Guest",
                Customer = customer,
                OrderProducts = model.CartItems.Select(item => new OrderProduct
                {
                    ProductId = item.ProductId,
                    ProductQuantity = item.CartQuantity
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Confirmation");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderProductViewModel>>("Cart") ?? new List<OrderProductViewModel>();
            var itemToRemove = cart.FirstOrDefault(p => p.ProductId == id);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("ViewCart");
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}