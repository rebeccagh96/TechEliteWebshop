namespace TechElite.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? ZipCode { get; set; }

        public string? City { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
