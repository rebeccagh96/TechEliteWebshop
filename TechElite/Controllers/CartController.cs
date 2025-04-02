using Microsoft.AspNetCore.Mvc;
using TechElite.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace TechElite.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "CartSessionKey";

        // Get Cart from Session
        private Cart GetCart()
        {
            var cart = HttpContext.Session.GetString(CartSessionKey);
            if (cart == null)
            {
                return new Cart();
            }
            return JsonConvert.DeserializeObject<Cart>(cart);
        }

        // Save Cart to Session
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));
        }

        // Display Cart
        public IActionResult Index()
        {
            var cart = GetCart();
            var itemCount = cart.Items.Sum(i => i.Quantity); // Calculate total number of items
            ViewBag.CartItemCount = itemCount;  // Pass item count to the view
            return View(cart);
        }

        // Add an item to the cart
        public IActionResult AddToCart(int productId, string name, decimal price, int quantity = 1)
        {
            var cart = GetCart();
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Name = name,
                    Price = price,
                    Quantity = quantity
                });
            }

            SaveCart(cart); // Save the updated cart to session
            return RedirectToAction("Index");
        }

        // Remove an item from the cart
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (itemToRemove != null)
            {
                cart.Items.Remove(itemToRemove);
                SaveCart(cart); // Save the updated cart to session
            }

            return RedirectToAction("Index");
        }

        // Clear the cart
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey); // Remove cart from session
            return RedirectToAction("Index");
        }
    }
}
