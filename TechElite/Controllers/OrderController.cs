using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechElite.Models;
using TechElite.Areas.Identity.Data;
using System.Threading.Tasks;
using TechElite.Helpers;
using static NuGet.Packaging.PackagingConstants;
using Microsoft.AspNetCore.Identity;


namespace TechElite.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<IActionResult> Checkout(string firstname, string lastname, string address, string zipcode, string city)
        {
            if (!ModelState.IsValid)
            {
                return View("ViewCart"); // Show validation messages
            }

            var user = await _userManager.GetUserAsync(User);

            // Check if the customer already exists
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

            var order = new Order
            {
                OrderDate = DateTime.Now,
                UserName = user.UserName,
                Customer = customer,
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
