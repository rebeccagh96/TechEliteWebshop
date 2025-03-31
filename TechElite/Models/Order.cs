namespace TechElite.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public int ProductPrice { get; set; }
        public int Total { get; set; }
        public required string ShippingMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; } // Vi hade skrivit string inte int
        public Customer Customer { get; set; } = default!;
        public ICollection<Product>? Products { get; set; } // Saknade navigation property
    }
}
