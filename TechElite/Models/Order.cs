namespace TechElite.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public int ProductPrice { get; set; }
        public int Total { get; set; }
        public string? ShippingMethod { get; set; }
        public DateTime OrderDate { get; set; }
        //public required string CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;

    }
}
