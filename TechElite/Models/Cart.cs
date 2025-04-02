namespace TechElite.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        // Calculate the total price of the cart
        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
