using Microsoft.AspNetCore.Mvc;
using TechElite.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;

namespace TechElite.Controllers
{
    public class OrderController : Controller
    {
        private const string CartSessionKey = "CartSessionKey";

        // Get Cart from Session
        private Cart GetCart()
        {
            var cart = HttpContext.Session.GetString(CartSessionKey);
            if (cart == null)
            {
                return new Cart();  // Return a new empty cart if there is no cart in the session
            }
            return JsonConvert.DeserializeObject<Cart>(cart);  // Deserialize the cart object from session
        }

        // Save Cart to Session
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));  // Save the cart to session
        }

        // Add Item to Cart - Add to cart logic
        public IActionResult AddToCart(int productId, string name, decimal price, int quantity = 1)
        {
            var cart = GetCart();  // Retrieve the cart from session
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);  // Check if the item already exists in the cart

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;  // Increase the quantity if item exists
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

            SaveCart(cart);  // Save the updated cart back to the session

            // Redirect to the Home page or wherever you want the user to go after adding to cart
            return RedirectToAction("Index", "Home");  // Redirect back to Home after adding to cart
        }

        // Display Order Summary - Display cart content and order form
        public IActionResult Index()
        {
            var cart = GetCart();  // Retrieve the cart
            if (!cart.Items.Any())
            {
                return RedirectToAction("Index", "Home");  // Redirect to Home if the cart is empty
            }

            // Calculate total price of the items in the cart
            var totalPrice = cart.Items.Sum(item => item.Price * item.Quantity);

            ViewBag.TotalPrice = totalPrice;  // Pass the total price to the view

            return View(cart);  // Pass the cart to the view
        }

        // Place the Order - Save the order and clear the cart
        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            var cart = GetCart();  // Retrieve the cart
            if (!cart.Items.Any())
            {
                return RedirectToAction("Index", "Home");  // Redirect to Home if the cart is empty
            }

            // Save the order details
            var newOrder = new Order
            {
                OrderDate = DateTime.Now,
                Total = cart.Items.Sum(item => item.Price * item.Quantity),  // Calculate the total order price
                ShippingMethod = order.ShippingMethod,
                UserId = order.UserId
            };

            // Normally, save the order to a database here
            // _orderService.SaveOrder(newOrder);

            // Clear the cart after placing the order
            HttpContext.Session.Remove(CartSessionKey);

            return RedirectToAction("Confirmation");  // Redirect to the order confirmation page
        }

        // Order Confirmation - Display a confirmation message after order is placed
        public IActionResult Confirmation()
        {
            return View();  // Return the confirmation view
        }
    }
}
