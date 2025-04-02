namespace TechElite.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public required int ProductId { get; set; }
        public required string ProductName { get; set; } = string.Empty;
        public int Amount { get; set; }
        public int ProductPrice { get; set; }
        public int Total { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Delivered { get; set; }
        public required string ShippingMethod { get; set; }
        public required int CustomerId { get; set; } // Vi hade skrivit string inte int
        public Customer Customer { get; set; } = default!;

    }
}
