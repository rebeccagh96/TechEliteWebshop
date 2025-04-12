using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechElite.Models;
using TechElite.Areas.Identity.Data;
using System.Threading.Tasks;

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
                .Include(o => o.Products)
                .ToListAsync();

            var customers = await _context.Customers.ToListAsync();
            var products = await _context.Products.ToListAsync();

            var orderViewModels = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                orderViewModels.Add(new OrderViewModel
                {
                    OrderId = order.OrderId,
                    CustomerId = order.CustomerId,
                    UserName = order.UserName,
                    Products = order.Products.Select(p => new ProductViewModel
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        Quantity = p.Quantity,
                        Price = p.Price,
                        Description = p.Description,
                        DepartmentId = p.DepartmentId
                    }).ToList(),
                    TotalPrice = order.Products.Sum(p => p.Price * p.Quantity), // Dynamically calculate TotalPrice
                    OrderDate = order.OrderDate
                });
            }

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
        public async Task<IActionResult> Save(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { message = "Ogiltig data", errors });
            }

            var order = new Order
            {
                OrderId = model.OrderId,
                CustomerId = model.CustomerId,
                UserName = model.UserName,
                Products = model.Products.Select(p => new Product
                {
                    ProductId = p.ProductId ?? 0,
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Description = p.Description,
                    DepartmentId = p.DepartmentId
                }).ToList(),
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Ogiltig data" });
            }

            var order = await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.OrderId == model.OrderId);

            if (order == null)
            {
                return NotFound("Ordern hittades inte.");
            }

            foreach (var product in model.Products)
            {
                var existingProduct = order.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.Quantity = product.Quantity;
                }
            }

            var productIdsToRemove = model.Products
                .Where(p => p.Quantity == 0)
                .Select(p => p.ProductId)
                .ToList();

            order.Products = order.Products
                .Where(p => !productIdsToRemove.Contains(p.ProductId))
                .ToList();


            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Ordern uppdaterades." });
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Ordern hittades inte.");
            }

            _context.Orders.Remove(order);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Ordern raderades framgångsrikt." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Ett fel inträffade: " + ex.Message });
            }
        }
    }
}
