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

            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "1",
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@techelite.com",
                    NormalizedEmail = "ADMIN@TECHELITE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin123!"),
                    SecurityStamp = string.Empty,
                    Role = "Admin"
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "Admin2",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@techelite.com",
                    NormalizedEmail = "ADMIN@TECHELITE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin123!"),
                    SecurityStamp = string.Empty,
                    Role = "Admin"
                },
                new ApplicationUser
                {
                    Id = "3",
                    UserName = "User",
                    NormalizedUserName = "USER",
                    Email = "user@techelite.com",
                    NormalizedEmail = "USER@TECHELITE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "User123!"),
                    SecurityStamp = string.Empty,
                    Role = "User"
                },
                new ApplicationUser
                {
                    Id = "4",
                    UserName = "User2",
                    NormalizedUserName = "USER2",
                    Email = "user2@techelite.com",
                    NormalizedEmail = "USER2@TECHELITE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "User123!"),
                    SecurityStamp = string.Empty,
                    Role = "User"
                });

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    UserId = "3",
                    FirstName = "User",
                    LastName = "Userson",
                    Address = "User street 1",
                    ZipCode = "12345",
                    City = "User city"
                },
                new Customer
                {
                    CustomerId = 2,
                    UserId = "4",
                    FirstName = "User2",
                    LastName = "Userson2",
                    Address = "User street 2",
                    ZipCode = "54321",
                    City = "User city"
                });

            modelBuilder.Entity<ProductDepartment>().HasData(
                new ProductDepartment
                {
                    ProductDepartmentId = 1,
                    DepartmentName = "Datorer & Skärmar"
                },
                new ProductDepartment
                {
                    ProductDepartmentId = 2,
                    DepartmentName = "Telefoner & Tablets"
                },
                new ProductDepartment
                {
                    ProductDepartmentId = 3,
                    DepartmentName = "Hörlurar & HiFi"
                },
                new ProductDepartment
                {
                    ProductDepartmentId = 4,
                    DepartmentName = "Tillbehör & Komponenter"
                },
                new ProductDepartment
                {
                    ProductDepartmentId = 5,
                    DepartmentName = "Gaming"
                });

            byte[] laptop1 = File.ReadAllBytes("wwwroot/images/laptop/laptop1.jpg");
            byte[] laptop2 = File.ReadAllBytes("wwwroot/images/laptop/laptop2.jpg");
            byte[] monitor1 = File.ReadAllBytes("wwwroot/images/monitor/monitor1.jpg");
            byte[] monitor2 = File.ReadAllBytes("wwwroot/images/monitor/monitor2.jpg");
            byte[] phone1 = File.ReadAllBytes("wwwroot/images/phone/phone1.jpg");
            byte[] phone2 = File.ReadAllBytes("wwwroot/images/phone/phone2.jpg");
            byte[] phone3 = File.ReadAllBytes("wwwroot/images/phone/phone3.jpg");
            byte[] phone4 = File.ReadAllBytes("wwwroot/images/phone/phone4.jpg");
            byte[] tablet1 = File.ReadAllBytes("wwwroot/images/tablet/tablet1.jpg");
            byte[] mouse1 = File.ReadAllBytes("wwwroot/images/tablet/mouse1.jpg");
            byte[] mouse2 = File.ReadAllBytes("wwwroot/images/tablet/mouse2.jpg");
            byte[] keyboard1 = File.ReadAllBytes("wwwroot/images/tablet/keyboard1.jpg");
            byte[] keyboard2 = File.ReadAllBytes("wwwroot/images/tablet/keyboard2.jpg");
            byte[] keyboard3 = File.ReadAllBytes("wwwroot/images/tablet/keyboard3.jpg");
            byte[] pad1 = File.ReadAllBytes("wwwroot/images/tablet/pad1.jpg");
            byte[] headphones1 = File.ReadAllBytes("wwwroot/images/headphones/headphones1.jpg");
            byte[] headphones2 = File.ReadAllBytes("wwwroot/images/headphones/headphones2.jpg");
            byte[] earbuds1 = File.ReadAllBytes("wwwroot/images/headphones/earbuds1.jpg");
            byte[] buds2 = File.ReadAllBytes("wwwroot/images/headphones/buds2.jpg");
            byte[] buds3 = File.ReadAllBytes("wwwroot/images/gaming/buds3.jpg");
            byte[] buds4 = File.ReadAllBytes("wwwroot/images/gaming/buds4.jpg");
            byte[] drawingtablet1 = File.ReadAllBytes("wwwroot/images/tablet/dr-tablet1.jpg");
            byte[] drawingtablet2 = File.ReadAllBytes("wwwroot/images/tablet/dr-tablet2.jpg");
            byte[] controller1 = File.ReadAllBytes("wwwroot/images/gaming/controller1.jpg");
            byte[] controller2 = File.ReadAllBytes("wwwroot/images/gaming/controller2.jpg");
            byte[] controller3 = File.ReadAllBytes("wwwroot/images/gaming/controller3.jpg");
            byte[] console1 = File.ReadAllBytes("wwwroot/images/gaming/console1.jpg");
            byte[] console2 = File.ReadAllBytes("wwwroot/images/gaming/console2.jpg");
            byte[] console3 = File.ReadAllBytes("wwwroot/images/gaming/console3.jpg");
            byte[] exthdd = File.ReadAllBytes("wwwroot/images/gaming/ext-hdd1.jpg");
            byte[] fan1 = File.ReadAllBytes("wwwroot/images/gaming/fan1.jpg");
            byte[] gpu1 = File.ReadAllBytes("wwwroot/images/gaming/gpu1.jpg");
            byte[] gpu2 = File.ReadAllBytes("wwwroot/images/gaming/gpu2.jpg");
            byte[] gpu3 = File.ReadAllBytes("wwwroot/images/gaming/gpu3.jpg");
            byte[] gpu4 = File.ReadAllBytes("wwwroot/images/gaming/gpu4.jpg");
            byte[] charger1 = File.ReadAllBytes("wwwroot/images/gaming/charger1.jpg");
            byte[] charger2 = File.ReadAllBytes("wwwroot/images/gaming/charger2.jpg");
            byte[] plug1 = File.ReadAllBytes("wwwroot/images/gaming/plug1.jpg");
            byte[] plug2 = File.ReadAllBytes("wwwroot/images/gaming/plug2.jpg");



            modelBuilder.Entity<Product>().HasData(
                  new Product
                  {
                      ProductId = 1,
                      ProductName = "Laptop",
                      ProductDescription = "16-tums bärbar dator från Apple",
                      Image = ,
                      ProductDepartmentId = 1,
                      Stock = 10,
                      Price = 23990
                  }
                );
        }




    }


}
