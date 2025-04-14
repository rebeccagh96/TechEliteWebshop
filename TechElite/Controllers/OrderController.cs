using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechElite.Models;
using TechElite.Areas.Identity.Data;
using System.Threading.Tasks;
using TechElite.Helpers;

namespace TechElite.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .ToListAsync();

            var customers = await _context.Customers.ToListAsync();
            var products = await _context.Products.ToListAsync();

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
                    ProductQuantity = op.ProductQuantity
                }).ToList(),
                TotalPrice = order.OrderProducts.Sum(op => op.Product.Price * op.ProductQuantity)
            }).ToList();

            var model = new AdminAccountViewModel
            {
                Orders = orderViewModels,
                Customers = customers,
                Products = products.Select(p => new ProductViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Description = p.Description,
                    DepartmentId = p.DepartmentId
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartPageViewModel model)
        {
            if (model == null || model.CartItems == null || !model.CartItems.Any())
            {
                return RedirectToAction("ViewCart", "Cart");
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
            return RedirectToAction("Confirmation", "Cart");
        }
    }
}
