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
                TotalPrice = order.TotalPrice
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
                OrderDate = DateTime.Now,
                OrderProducts = model.OrderProducts.Select(op => new OrderProduct
                {
                    ProductId = op.ProductId,
                    ProductQuantity = op.ProductQuantity
                }).ToList()
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
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.OrderId == model.OrderId);

            if (order == null)
            {
                return NotFound("Ordern hittades inte.");
            }

            // Update existing products in the order
            foreach (var orderProduct in model.OrderProducts)
            {
                var existingOrderProduct = order.OrderProducts.FirstOrDefault(op => op.ProductId == orderProduct.ProductId);
                if (existingOrderProduct != null)
                {
                    existingOrderProduct.ProductQuantity = orderProduct.ProductQuantity;
                }
                else
                {
                    // Add new product to the order
                    order.OrderProducts.Add(new OrderProduct
                    {
                        ProductId = orderProduct.ProductId,
                        ProductQuantity = orderProduct.ProductQuantity
                    });
                }
            }

            // Remove products with quantity 0
            var productIdsToRemove = model.OrderProducts
                .Where(op => op.ProductQuantity == 0)
                .Select(op => op.ProductId)
                .ToList();

            order.OrderProducts = order.OrderProducts
                .Where(op => !productIdsToRemove.Contains(op.ProductId))
                .ToList();

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Ordern uppdaterades." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
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
