namespace TechElite.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int ProductPrice { get; set; }
        public decimal Total { get; set; }  // Change this to decimal instead of int
        public string? ShippingMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public string? UserId { get; set; }
    }
}

