using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechElite.Areas.Identity.Data;
using TechElite.Models;

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

        // Konfigurerar relationer
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

        // Skapa statiska variabler för att kunna referera till dem vid relationer
        var dept1Id = "DPT-00000001";
        var dept2Id = "DPT-00000002";
        var dept3Id = "DPT-00000003";
        var dept4Id = "DPT-00000004";
        var dept5Id = "DPT-00000005";

        modelBuilder.Entity<Department>().HasData(
            new Department
            {
                DepartmentId = dept1Id,
                DepartmentName = "Telefoner & Tablets",
                DepartmentDescription = "Telefoner och surfplattor för alla syften"
            },
            new Department
            {
                DepartmentId = dept2Id,
                DepartmentName = "Laptops & Skärmar",
                DepartmentDescription = "Laptops och skärmar för både hemmet och kontoret"
            },
            new Department
            {
                DepartmentId = dept3Id,
                DepartmentName = "Hörlurar & Hifi",
                DepartmentDescription = "Hörlurar och hifi-utrustning"
            },
            new Department
            {
                DepartmentId = dept4Id,
                DepartmentName = "Tillbehör & komponenter",
                DepartmentDescription = "Tillbehör och komponenter för alla behov"
            },
            new Department
            {
                DepartmentId = dept5Id,
                DepartmentName = "Gaming",
                DepartmentDescription = "Gaming-utrustning och tillbehör"
            }
        );

        var cat1Id = "CAT-00000001";
        var cat2Id = "CAT-00000002";
        var cat3Id = "CAT-00000003";
        var cat4Id = "CAT-00000004";
        var cat5Id = "CAT-00000005";
        var cat6Id = "CAT-00000006";

        modelBuilder.Entity<ForumCategory>().HasData(
            new ForumCategory
            {
                CategoryId = cat1Id,
                CategoryName = "Rekommendationer",
                Description = "Rekommendera dina favoriter"
            },
            new ForumCategory
            {
                CategoryId = cat2Id,
                CategoryName = "Tips och hjälp",
                Description = "Be om hjälp"
            },
            new ForumCategory
            {
                CategoryId = cat3Id,
                CategoryName = "Produkter",
                Description = "Diskutera produkter"
            },
            new ForumCategory
            {
                CategoryId = cat4Id,
                CategoryName = "Support",
                Description = "Få support"
            },
            new ForumCategory
            {
                CategoryId = cat5Id,
                CategoryName = "Köp & Sälj",
                Description = "Köp och sälj produkter"
            },
            new ForumCategory
            {
                CategoryId = cat6Id,
                CategoryName = "Övrigt",
                Description = "Diskutera övriga ämnen"
            }
        );

        var product1Id = "PRD-00000001";
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = product1Id,
                DepartmentId = dept1Id,
                ProductName = "Exempelprodukt",
                Description = "Beskrivning av exempelprodukten",
                Price = 100,
                Quantity = 10
            }
        );

        // För att seeda Customer, ForumThread, ForumReply, Review och Order
        // behöver vi referera till en seedad användare. Vi sätter statiska ID:n för användare.
        // Användare med ID "USER1-STATIC-ID" kommer att användas.
        var user1StaticId = "USER1-STATIC-ID";
        var customer1StaticId = "CUSTOMER1-STATIC-ID";

        // Seed en användare
        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = user1StaticId,
                UserName = "user1",
                NormalizedUserName = "USER1",
                Email = "user1@example.com",
                NormalizedEmail = "USER1@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = string.Empty,
                ConcurrencyStamp = string.Empty,
                FirstName = "Anna",
                LastName = "Andersson",
                CustomUserId = user1StaticId
            }
        );

        // Seed en Customer (kopplad till user1)
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                CustomerId = customer1StaticId,
                FirstName = "Anna",
                LastName = "Andersson",
                Address = "Exempelgatan 1",
                ZipCode = "12345",
                City = "Exempelstad",
                CustomUserId = user1StaticId,
                UserName = "user1"
            }
        );

        // Seed en ForumThread
        var thread1Id = "THR-00000001";
        modelBuilder.Entity<ForumThread>().HasData(
            new ForumThread
            {
                ThreadId = thread1Id,
                ThreadTitle = "Välkommen till forumet",
                ThreadContent = "Detta är den första tråden.",
                PublishDate = new DateTime(2025, 1, 1),
                CategoryId = cat1Id,
                CustomUserId = user1StaticId,
                UserName = "user1"
            }
        );

        // Seed en ForumReply
        var reply1Id = "RPL-00000001";
        modelBuilder.Entity<ForumReply>().HasData(
            new ForumReply
            {
                ReplyId = reply1Id,
                Content = "Tack för välkomnandet!",
                PublishDate = new DateTime(2025, 1, 1),
                ThreadId = thread1Id,
                CustomUserId = user1StaticId,
                UserName = "user1"
            }
        );

        // Seed en Review
        var review1Id = "REV-00000001";
        modelBuilder.Entity<Review>().HasData(
            new Review
            {
                ReviewId = review1Id,
                ProductId = product1Id,
                ReviewerName = "Anna",
                ReviewTitle = "Bra produkt",
                Rating = 5,
                ReviewText = "Jag gillar den verkligen!",
                ReviewDate = new DateTime(2025, 1, 1),
                CustomUserId = user1StaticId
            }
        );

        // Seed en Order
        var order1Id = "ORD-00000001";
        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                OrderId = order1Id,
                CustomerId = customer1StaticId,
                UserName = "user1",
                OrderDate = new DateTime(2025, 1, 1),
                ProductName = "Exempelprodukt",
                TotalPrice = 100
            }
        );
    }

    public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Seed roller
        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seedar användare med statiska ID:n
        var user1 = new ApplicationUser
        {
            Id = "USER1-STATIC-ID",
            UserName = "user1",
            Email = "user1@example.com",
            FirstName = "Anna",
            LastName = "Andersson",
            CustomUserId = "USER1-STATIC-ID",
            EmailConfirmed = true
        };
        var user2 = new ApplicationUser
        {
            Id = "USER2-STATIC-ID",
            UserName = "user2",
            Email = "user2@example.com",
            FirstName = "Bernt",
            LastName = "Berntsson",
            CustomUserId = "USER2-STATIC-ID",
            EmailConfirmed = true
        };
        var admin1 = new ApplicationUser
        {
            Id = "ADMIN1-STATIC-ID",
            UserName = "admin1",
            Email = "admin1@example.com",
            FirstName = "Admin",
            LastName = "Adminson",
            CustomUserId = "ADMIN1-STATIC-ID",
            EmailConfirmed = true
        };

        var users = new[] { user1, user2, admin1 };

        foreach (var user in users)
        {
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                await userManager.CreateAsync(user, "Password123!");
                // Tilldela roller: om användarnamnet innehåller "admin" får användaren Admin-rollen, annars User
                // Vi behöver fixa så att användare som registrerar sig via formuläret inte kan använda
                // namnet admin
                if (user.UserName.ToLower().Contains("admin"))
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
