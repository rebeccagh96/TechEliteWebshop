namespace TechElite.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }
        public required string ZipCode { get; set; }
        public required string City { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ApplicationUser User { get; set; } = default!;
    }
}
