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

            var user = await _userManager.GetUserAsync(User);

            // Retrieve the existing customer or create a new one
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
            if (customer == null)
            {
                // Create a new customer if one doesn't exist
                customer = new Customer
                {
                    FirstName = firstname,
                    LastName = lastname,
                    Address = address,
                    ZipCode = zipcode,
                    City = city,
                    ApplicationUserId = user.Id,
                    UserName = user.UserName
                };
                _context.Customers.Add(customer);
            }
            else
            {
                // Update existing customer details if necessary
                customer.FirstName = firstname;
                customer.LastName = lastname;
                customer.Address = address;
                customer.ZipCode = zipcode;
                customer.City = city;
            }

            var cartItems = HttpContext.Session.GetObjectFromJson<List<OrderProductViewModel>>("Cart");
            if (cartItems == null || !cartItems.Any())
            {
                ModelState.AddModelError("", "Din kundvagn är tom.");
                return View("ViewCart");
            }

            // Create a new order and associate it with the existing or new customer
            var order = new Order
            {
                OrderDate = DateTime.Now,
                UserName = user.UserName,
                Customer = customer, // Associate the customer
                OrderProducts = cartItems.Select(item => new OrderProduct
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
            var model = new AdminAccountViewModel
            {
                Orders = new List<OrderViewModel>() 
            };
            return View(model);
        }
    }
}