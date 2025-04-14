using Microsoft.AspNetCore.Mvc;
using TechElite.Helpers.TechElite.Helpers;
using TechElite;
using TechElite.Models;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult AddToCart(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product == null) return NotFound();

        var cart = HttpContext.Session.GetObjectFromJson<List<CartViewModel>>("Cart") ?? new List<CartViewModel>();
        var existing = cart.FirstOrDefault(c => c.ProductId == id);

        if (existing != null)
        {
            existing.CartQuantity++;
        }
        else
        {
            cart.Add(new CartViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                Quantity = 1
            });
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);
        return RedirectToAction("ViewCart");
    }

    public IActionResult ViewCart()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartViewModel>>("Cart") ?? new List<CartViewModel>();
        return View(cart);
    }

    public IActionResult RemoveFromCart(int id)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartViewModel>>("Cart");
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
