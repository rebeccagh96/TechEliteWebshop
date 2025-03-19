namespace TechElite.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Amount { get; set; }
        public int ProductPrice { get; set; }
        public int Total { get; set; }
        public string ShippingMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
    }
}
