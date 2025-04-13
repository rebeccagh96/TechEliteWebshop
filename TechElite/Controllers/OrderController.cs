using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechElite.Models;

[HttpPost]
public IActionResult Checkout()
{
    var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
    if (cart == null || !cart.Any())
        return RedirectToAction("ViewCart", "Cart");

    var order = new Order
    {
        OrderDate = DateTime.Now,
        UserName = User.Identity?.Name ?? "Guest",
        OrderProducts = new List<OrderProduct>()
    };

    foreach (var item in cart)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
        if (product == null || product.Stock < item.Quantity)
        {
            // Om inte tillräckligt i lager
            return RedirectToAction("ViewCart", "Cart");
        }

        product.Stock -= item.Quantity;

        order.OrderProducts.Add(new OrderProduct
        {
            ProductId = product.ProductId,
            ProductQuantity = item.Quantity
        });
    }

    _context.Orders.Add(order);
    _context.SaveChanges();

    HttpContext.Session.Remove("Cart");
    return RedirectToAction("Index");
}
