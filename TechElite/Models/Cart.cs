namespace TechElite.Models
{
    
    
        public class Cart
        {
            public List<CartItem> Items { get; set; } = new List<CartItem>();

            public decimal TotalPrice
            {
                get
                {
                    return Items.Sum(item => item.Price * item.Quantity);
                }
            }
        }
    

}
