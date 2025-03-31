using Microsoft.AspNetCore.Identity;

namespace TechElite.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string Role { get; set; } = "User";
        public ICollection<Customer>? Customers { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<ForumThread>? Threads { get; set; }
        public ICollection<Reply>? Replies { get; set; }
    }
}
