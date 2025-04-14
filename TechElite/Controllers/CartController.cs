using Microsoft.AspNetCore.Mvc;
using TechElite.Helpers.TechElite.Helpers;
using TechElite;
using TechElite.Models;
using Microsoft.EntityFrameworkCore;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
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
                ProductQuantity = op.ProductQuantity,
                CartQuantity = op.ProductQuantity // Assuming you want to show the quantity in the cart as well
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
                ProductQuantity = 1
            });
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);
        return RedirectToAction("ViewCart");
    }

    public IActionResult ViewCart()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<OrderProductViewModel>>("Cart") ?? new List<OrderProductViewModel>();
        return View(cart);
    }

    public IActionResult RemoveFromCart(int id)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<OrderProduct>>("Cart");
        if (cart != null)
        {
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
        }
        return RedirectToAction("ViewCart");
    }
}
