using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechElite.Models;

namespace TechElite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TechElite.Models.User> User { get; set; } = default!;

        public DbSet<ForumCategory> ForumCategorys { get; set; }
        public DbSet<Models.Thread> Threads { get; set; }
        public DbSet<Reply> Replys { get; set; }
    }
}
