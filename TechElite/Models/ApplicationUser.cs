using Microsoft.AspNetCore.Identity;

namespace TechElite.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Customer>? Customers { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<ForumThread>? Threads { get; set; }
        public ICollection<Reply>? Replies { get; set; }
    }
}
