namespace TechElite.Models
{
    
        public class CartViewModel
        {
            public List<CartItem> CartItems { get; set; } = new List<CartItem>();
            public decimal TotalPrice { get; set; }
            public int ItemCount { get; set; }

            public CartViewModel(List<CartItem> items)
            {
                CartItems = items;
                TotalPrice = items.Sum(item => item.Price * item.Quantity);
                ItemCount = items.Sum(item => item.Quantity);
            }
        }

    
}
