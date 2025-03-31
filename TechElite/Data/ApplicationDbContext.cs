using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using TechElite.Models;

namespace TechElite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ForumThread> ForumThreads { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ForumCategory> ForumCategories { get; set; }
        public DbSet<ProductDepartment> ProductDepartments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithMany(u => u.Customers)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ForumThread>()
                .HasOne(ft => ft.User)
                .WithMany(u => u.Threads)
                .HasForeignKey(ft => ft.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ForumThread>()
                .HasOne(ft => ft.ForumCategory)
                .WithMany(fc => fc.Threads)
                .HasForeignKey(ft => ft.ForumCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.User)
                .WithMany(u => u.Replies)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Thread)
                .WithMany(ft => ft.Replies)
                .HasForeignKey(r => r.ThreadId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.ForumCategory)
                .WithMany(fc => fc.Replies)
                .HasForeignKey(r => r.ForumCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductDepartment)
                .WithMany(pd => pd.Products)
                .HasForeignKey(p => p.ProductDepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seedar db

            //var hasher = new PasswordHasher<ApplicationUser>();
            //modelBuilder.Entity<ApplicationUser>().HasData(
            //    new ApplicationUser
            //    {
            //        Id = "1",
            //        UserName = "Admin",
            //        NormalizedUserName = "ADMIN",
            //        Email = "admin@techelite.com",
            //        NormalizedEmail = "ADMIN@TECHELITE.COM",
            //        EmailConfirmed = true,
            //        PasswordHash = hasher.HashPassword(null, "Admin123!"),
            //        SecurityStamp = string.Empty,
            //        Role = "Admin"
            //    },
            //    new ApplicationUser
            //    {
            //        Id = "2",
            //        UserName = "Admin2",
            //        NormalizedUserName = "ADMIN",
            //        Email = "admin@techelite.com",
            //        NormalizedEmail = "ADMIN@TECHELITE.COM",
            //        EmailConfirmed = true,
            //        PasswordHash = hasher.HashPassword(null, "Admin123!"),
            //        SecurityStamp = string.Empty,
            //        Role = "Admin"
            //    },
            //    new ApplicationUser
            //    {
            //        Id = "3",
            //        UserName = "User",
            //        NormalizedUserName = "USER",
            //        Email = "user@techelite.com",
            //        NormalizedEmail = "USER@TECHELITE.COM",
            //        EmailConfirmed = true,
            //        PasswordHash = hasher.HashPassword(null, "User123!"),
            //        SecurityStamp = string.Empty,
            //        Role = "User"
            //    },
            //    new ApplicationUser
            //    {
            //        Id = "4",
            //        UserName = "User2",
            //        NormalizedUserName = "USER2",
            //        Email = "user2@techelite.com",
            //        NormalizedEmail = "USER2@TECHELITE.COM",
            //        EmailConfirmed = true,
            //        PasswordHash = hasher.HashPassword(null, "User123!"),
            //        SecurityStamp = string.Empty,
            //        Role = "User"
            //    });

            //modelBuilder.Entity<Customer>().HasData(
            //    new Customer
            //    {
            //        CustomerId = 1,
            //        UserId = "3",
            //        FirstName = "User",
            //        LastName = "Userson",
            //        Address = "User street 1",
            //        ZipCode = "12345",
            //        City = "User city"
            //    },
            //    new Customer
            //    {
            //        CustomerId = 2,
            //        UserId = "4",
            //        FirstName = "User2",
            //        LastName = "Userson2",
            //        Address = "User street 2",
            //        ZipCode = "54321",
            //        City = "User city"
            //    });

            //modelBuilder.Entity<Order>().HasData(
            //    new Order
            //    {
            //        OrderId = 1,
            //        CustomerId = 1,
            //        Amount = 5,
            //        ProductPrice = 100,
            //        OrderDate = DateTime.Now,
            //        Delivered = true
            //    },
            //    new Order
            //    {
            //        OrderId = 2,
            //        CustomerId = 2,
            //        OrderDate = DateTime.Now,
            //        Delivered = false

            //    });

        }




    }


}
