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
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfigurerar relationer
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.ApplicationUser)
            .WithOne(u => u.Customer)
            .HasForeignKey<Customer>(c => c.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.ApplicationUser)
            .WithMany()
            .HasForeignKey(r => r.ApplicationUserId)
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
            .HasForeignKey(ft => ft.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<ForumReply>()
            .HasOne(fr => fr.Thread)
            .WithMany(ft => ft.Replies)
            .HasForeignKey(fr => fr.ThreadId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ForumReply>()
            .HasOne(fr => fr.ApplicationUser)
            .WithMany()
            .HasForeignKey(fr => fr.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.ApplicationUser)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.ForumThread)
            .WithMany(ft => ft.Notifications)
            .HasForeignKey(n => n.ThreadId)
            .OnDelete(DeleteBehavior.Restrict);

        // Skapa statiska variabler för att kunna referera till dem vid relationer
        var dept1Id = 1;
        var dept2Id = 2;
        var dept3Id = 3;
        var dept4Id = 4;
        var dept5Id = 5;

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

        var cat1Id = 1;
        var cat2Id = 2;
        var cat3Id = 3;
        var cat4Id = 4;
        var cat5Id = 5;
        var cat6Id = 6;

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

        var product1Id = 1;
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
        var customer1StaticId = 1;

        // Seedar en användare
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
            }
        );

        // Seeda en Customer (kopplad till user1)
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                CustomerId = customer1StaticId,
                FirstName = "Anna",
                LastName = "Andersson",
                Address = "Exempelgatan 1",
                ZipCode = "12345",
                City = "Exempelstad",
                ApplicationUserId = user1StaticId,
                UserName = "user1"
            }
        );

        // Seeda en ForumThread
        var thread1Id = 1;
        modelBuilder.Entity<ForumThread>().HasData(
            new ForumThread
            {
                ThreadId = thread1Id,
                ThreadTitle = "Välkommen till forumet",
                ThreadContent = "Detta är den första tråden.",
                PublishDate = new DateTime(2025, 1, 1),
                CategoryId = cat1Id,
                UserName = "user1",
                ApplicationUserId = user1StaticId // Referens till den seedade användaren

            }
        );

        // Seeda en ForumReply
        var reply1Id = 1;
        modelBuilder.Entity<ForumReply>().HasData(
            new ForumReply
            {
                ReplyId = reply1Id,
                Content = "Tack för välkomnandet!",
                PublishDate = new DateTime(2025, 1, 1),
                ThreadId = thread1Id,
                UserName = "user1",
                ApplicationUserId = user1StaticId // Referens till den seedade användaren

            }
        );

        // Seeda en Review
        var review1Id = 1;
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
                ApplicationUserId = user1StaticId // Referens till den seedade användaren
            }
        );

        // Seeda en Order
        var order1Id = 1;
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

        // Seeda roller
        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    Console.WriteLine($"Roll kunde inte skapas {role}: {errors}");
                }
            }
        }

        // Seedar användare med statiska ID:n (Identity ID är string)
        var user1 = new ApplicationUser
        {
            Id = "USER1-STATIC-ID",
            UserName = "user1",
            Email = "user1@example.com",
            FirstName = "Anna",
            LastName = "Andersson",
            EmailConfirmed = true
        };
        var user2 = new ApplicationUser
        {
            Id = "USER2-STATIC-ID",
            UserName = "user2",
            Email = "user2@example.com",
            FirstName = "Bernt",
            LastName = "Berntsson",
            EmailConfirmed = true
        };
        var admin1 = new ApplicationUser
        {
            Id = "ADMIN1-STATIC-ID",
            UserName = "admin1",
            Email = "admin1@example.com",
            FirstName = "Admin",
            LastName = "Adminson",
            EmailConfirmed = true
        };

        var users = new[] { user1, user2, admin1 };

        foreach (var user in users)
        {
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                var createUserResult = await userManager.CreateAsync(user, "Password123!");
                if (createUserResult.Succeeded)
                {
                    var roleToAssign = user.UserName.Equals("admin1", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User";
                    var addRoleResult = await userManager.AddToRoleAsync(user, roleToAssign);
                    if (!addRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                        Console.WriteLine($"{user.UserName} kunde inte tilldelas rollen: {roleToAssign}.{errors}");
                    }
                }
                else
                {
                    var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                    Console.WriteLine($"{user.UserName} kunde inte skapas: {errors}");
                }

            }
        }
    }
}
