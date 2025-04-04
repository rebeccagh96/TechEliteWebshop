using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechEliteWebshop.Areas.Identity.Data;
using TechEliteWebshop.Models;

namespace TechElite;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<ForumCategory> ForumCategories { get; set; }
    public DbSet<ForumReply> ForumReplies { get; set; }
    public DbSet<ForumThread> ForumThreads { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.ApplicationUser)
            .WithOne(u => u.Customer)
            .HasForeignKey<Customer>(c => c.CustomUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.ApplicationUser)
            .WithMany()
            .HasForeignKey(r => r.CustomUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Department)
            .WithMany(d => d.Products)
            .HasForeignKey(p => p.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ForumThread>()
            .HasOne(ft => ft.Category)
            .WithMany(fc => fc.Threads)
            .HasForeignKey(ft => ft.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ForumThread>()
            .HasOne(ft => ft.ApplicationUser)
            .WithMany()
            .HasForeignKey(ft => ft.CustomUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ForumReply>()
            .HasOne(fr => fr.Thread)
            .WithMany(ft => ft.Replies)
            .HasForeignKey(fr => fr.ThreadId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ForumReply>()
            .HasOne(fr => fr.ApplicationUser)
            .WithMany()
            .HasForeignKey(fr => fr.CustomUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        modelBuilder.Entity<Department>().HasData(
            new Department
            {
                DepartmentId = "DPT-0001",
                DepartmentName = "Telefoner & Tablets",
                DepartmentDescription = "Telefoner och surfplattor för alla syften"
            },
            new Department
            {
                DepartmentId = "DPT-0002",
                DepartmentName = "Laptops & Skärmar",
                DepartmentDescription = "Laptops och skärmar för både hemmet och kontoret"
            }
        );

        modelBuilder.Entity<ForumCategory>().HasData(
            new ForumCategory
            {
                CategoryId = "CAT-0001",
                Name = "Rekommendationer",
                Description = "Rekommendera dina favoriter"
            },
            new ForumCategory
            {
                CategoryId = "CAT-0002",
                Name = "Tips och hjälp",
                Description = "Be om hjälp"
            }
        );
    }

    public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Seed roles
        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed users
        var users = new[]
        {
            new ApplicationUser
            {
                UserName = "user1",
                Email = "user1@example.com",
                FirstName = "Anna",
                LastName = "Andersson",
                CustomUserId = "USR-0001",
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                UserName = "user2",
                Email = "user2@example.com",
                FirstName = "Bernt",
                LastName = "Berntsson",
                CustomUserId = "USR-0002",
                EmailConfirmed = true
            }
        };

        foreach (var user in users)
        {
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                await userManager.CreateAsync(user, "Password123!");
                await userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}
