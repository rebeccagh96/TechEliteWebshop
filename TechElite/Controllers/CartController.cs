using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using TechElite.Models;
using TechElite.Areas.Identity.Data;
using TechElite.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace TechElite.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        // Add a product to the cart
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();

            var cart = HttpContext.Session.GetObjectFromJson<List<OrderProductViewModel>>("Cart") ?? new List<OrderProductViewModel>();
            var existing = cart.FirstOrDefault(c => c.ProductId == id);

            if (existing != null)
            {
                existing.CartQuantity++;
            }
            else
            {
                cart.Add(new OrderProductViewModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    CartQuantity = 1,
                    ProductQuantity = 1
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string firstname, string lastname, string address, string zipcode, string city)
        {
            if (!ModelState.IsValid)
            {
                return View("ViewCart"); // Show validation messages
            }
            var FirstName = firstname;
            var LastName = lastname;
            var Address = address;
            var Zipcode = zipcode;
            var City = city;
            var user = await _userManager.GetUserAsync(User);

            var customer = new Customer
            {
                FirstName = FirstName,
                LastName = LastName,
                Address = Address,
                ZipCode = Zipcode,
                City = City,
                ApplicationUserId = user.Id,
                UserName = user.UserName
            };

            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                ModelState.AddModelError("", "Din kundvagn är tom.");
                return View("ViewCart");
            }

            var cartItems = JsonSerializer.Deserialize<List<Product>>(cartJson);
            var order = new Order
            {
                OrderDate = DateTime.Now,
                UserName = user.UserName,
                Customer = customer,
                OrderProducts = cartItems.Select(item => new OrderProduct
                {
                    ProductId = item.ProductId,
                    ProductQuantity = item.Quantity,
                }).ToList()
            };

            _context.Customers.Add(customer);
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