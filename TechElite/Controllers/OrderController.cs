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

[HttpPost]
public IActionResult Checkout()
{
    var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
    if (cart == null || !cart.Any())
        return RedirectToAction("ViewCart", "Cart");

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
        public async Task<IActionResult> Edit([FromBody] OrderEditDto model)
        {
            if (model == null)
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

            foreach (var orderProduct in model.OrderProducts)
            {
                var existingOrderProduct = order.OrderProducts.FirstOrDefault(op => op.ProductId == orderProduct.ProductId);
                if (existingOrderProduct != null)
                {
                    existingOrderProduct.ProductQuantity = orderProduct.ProductQuantity;
                }
                else
                {
                    order.OrderProducts.Add(new OrderProduct
                    {
                        ProductId = orderProduct.ProductId,
                        ProductQuantity = orderProduct.ProductQuantity
                    });
                }
            }

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
        public async Task<IActionResult> Delete([FromBody] OrderDeleteDto request)
        {
            if (request == null || request.OrderId <= 0)
            {
                return BadRequest("Order ID krävs.");
            }

            int orderId = request.OrderId;

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
