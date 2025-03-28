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

        public DbSet<Customer> Customer { get; set; }
        public DbSet<ForumCategory> ForumCategory { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductDepartment> ProductDepartment { get; set; }
        public DbSet<Reply> Reply { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Models.Thread> Thread { get; set; }
        public DbSet<TechElite.Models.User> User { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define composite key and relationships
            modelBuilder.Entity<Product>()
                .HasKey(p => new { p.ProductId, p.ProductDepartmentId });

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => new { r.ProductId, r.ProductDepartmentId })
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductDepartment>()
                .HasMany(pd => pd.Products);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders);

            modelBuilder.Entity<Models.Thread>()
                .HasMany(r => r.Replies);

            modelBuilder.Entity<ForumCategory>()
                .HasMany(t => t.Threads);

            modelBuilder.Entity<Reply>()
                .HasKey(r => new { r.ReplyId, r.ThreadId });

            modelBuilder.Entity<Review>()
                .HasOne(p => p.Product)
                .HasForeignKey(r => new { r.ProductDepartmentId })
                .OnDelete(DeleteBehavior.Cascade);













        }


    }


}
