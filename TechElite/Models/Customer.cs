namespace TechElite.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string ZipCode { get; set; }
        public required string City { get; set; }
        public required ICollection<Order> Orders { get; set; }
        public ApplicationUser User { get; set; } = default!;
    }
}
