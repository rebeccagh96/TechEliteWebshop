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



            modelBuilder.Entity<Reply>()
                .HasOne(r => r.User)
                .WithMany(u => u.Replies)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Thread)
                .WithMany(ft => ft.Replies)
                .HasForeignKey(r => r.ThreadId)
                .OnDelete((DeleteBehavior)ReferentialAction.NoAction);


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
                    UserName = "User",
                    NormalizedUserName = "USER",
                    Email = "user@techelite.com",
                    NormalizedEmail = "USER@TECHELITE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "User123!"),
                    SecurityStamp = string.Empty,
                    Role = "User"
                });



        }




    }


}
