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
    public DbSet<UserContact> userContacts { get; set; }
    public DbSet<Cart> carts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

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

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId)
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
        var product2Id = 2;
        var product3Id = 3;
        var product4Id = 4;
        var product5Id = 5;
        var product6Id = 6;
        var product7Id = 7;
        var product8Id = 8;
        var product9Id = 9;
        var product10Id = 10;
        var product11Id = 11;
        var product12Id = 12;
        var product13Id = 13;
        var product14Id = 14;
        var product15Id = 15;
        var product16Id = 16;
        var product17Id = 17;
        var product18Id = 18;
        var product19Id = 19;
        var product20Id = 20;
        var product21Id = 21;
        var product22Id = 22;
        var product23Id = 23;
        var product24Id = 24;
        var product25Id = 25;

        var productDescription = "Upplev kraften i den senaste tekniken! " +
            "Designad för att leverera hög prestanda, " +
            "smart funktionalitet och stilren estetik.";

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = product1Id,
                DepartmentId = dept1Id,
                ProductName = "Elite Phone",
                Description = productDescription,
                Price = 100.00m,
                Quantity = 10
            },
            new Product
            {
                ProductId = product2Id,
                DepartmentId = dept2Id,
                ProductName = "Mac Attack",
                Description = productDescription,
                Price = 1990.00m,
                Quantity = 20
            },
            new Product
            {
                ProductId = product3Id,
                DepartmentId = dept3Id,
                ProductName = "Temu Earbuds",
                Description = productDescription,
                Price = 19.00m,
                Quantity = 15
            },
            new Product
            {
                ProductId = product4Id,
                DepartmentId = dept4Id,
                ProductName = "LADDARE X2000",
                Description = productDescription,
                Price = 19.00m,
                Quantity = 150
            },
            new Product
            {
                ProductId = product5Id,
                DepartmentId = dept5Id,
                ProductName = "Xbox Kontroll",
                Description = productDescription,
                Price = 199.00m,
                Quantity = 30
            },
            new Product
            {
                ProductId = product6Id,
                DepartmentId = dept1Id,
                ProductName = "Rit Bräda",
                Description = productDescription,
                Price = 19.00m,
                Quantity = 100
            },
            new Product
            {
                ProductId = product7Id,
                DepartmentId = dept2Id,
                ProductName = "Windows Fusion",
                Description = productDescription,
                Price = 1990.00m,
                Quantity = 10
            },
            new Product
            {
                ProductId = product8Id,
                DepartmentId = dept3Id,
                ProductName = "Professor Earbuds",
                Description = productDescription,
                Price = 19.00m,
                Quantity = 120
            },
            new Product
            {
                ProductId = product9Id,
                DepartmentId = dept4Id,
                ProductName = "Smart Charger",
                Description = productDescription,
                Price = 19.00m,
                Quantity = 35
            },
            new Product
            {
                ProductId = product10Id,
                DepartmentId = dept5Id,
                ProductName = "Custom ps5 Kontroll",
                Description = productDescription,
                Price = 199.00m,
                Quantity = 10
            },
            new Product
            {
                ProductId = product11Id,
                DepartmentId = dept1Id,
                ProductName = "Wish Rit Bräda",
                Description = productDescription,
                Price = 19.00m,
                Quantity = 15
            },
            new Product
            {
                ProductId = product12Id,
                DepartmentId = dept2Id,
                ProductName = "Elite Monitor",
                Description = productDescription,
                Price = 1990.00m,
                Quantity = 50
            },
            new Product
            {
                ProductId = product13Id,
                DepartmentId = dept3Id,
                ProductName = "Airpods pro",
                Description = productDescription,
                Price = 199.00m,
                Quantity = 20
            },
            new Product 
            {
                ProductId = product14Id,
                DepartmentId = dept4Id,
                ProductName = "Supercharger adapter",
                Description = productDescription,
                Price = 19.00m,
                Quantity = 80
            },
            new Product
            {
                ProductId = product15Id,
                DepartmentId = dept5Id,
                ProductName = "Standard PS Kontroll",
                Description = productDescription,
                Price = 199.00m,
                Quantity = 50
            },
            new Product
            {
                ProductId = product16Id,
                DepartmentId = dept1Id,
                ProductName = "iphone 7",
                Description = productDescription,
                Price = 1990.00m,
                Quantity = 30
            },
            new Product
            {
                ProductId = product17Id,
                DepartmentId = dept2Id,
                ProductName = "PS5",
                Description = productDescription,
                Price = 1990.00m,
                Quantity = 20
            },
            new Product 
            {
                ProductId = product18Id,
                DepartmentId = dept3Id,
                ProductName = "Trådade Ipods Pro",
                Description = productDescription,
                Price = 199.00m,
                Quantity = 20
            },
            new Product
            {
                ProductId = product19Id,
                DepartmentId = dept4Id,
                ProductName = "iphone adapter",
                Description = productDescription,
                Price = 19.00m,
                Quantity = 30
            },
            new Product
            {
                ProductId = product20Id,
                DepartmentId = dept5Id,
                ProductName = "Nintendo",
                Description = productDescription,
                Price = 1990.00m,
                Quantity = 20
            },
            new Product
            {
                ProductId = product21Id,
                DepartmentId = dept1Id,
                ProductName = "iphone 5",
                Description = productDescription,
                Price = 1990.00m,
                Quantity = 20
            },
            new Product
            {
                ProductId = product22Id,
                DepartmentId = dept2Id,
                ProductName = "Win Screen",
                Description = productDescription,
                Price = 1990.00m,
                Quantity = 20
            },
            new Product
            {
                ProductId = product23Id,
                DepartmentId = dept3Id,
                ProductName = "WIN-Win Headpones",
                Description = productDescription,
                Price = 199.00m,
                Quantity = 20
            },
            new Product
            {
                ProductId = product24Id,
                DepartmentId = dept4Id,
                ProductName = "Superb Fläkt",
                Description = productDescription,
                Price = 199.00m,
                Quantity = 25
            }, 
            new Product
            {
                ProductId = product25Id,
                DepartmentId = dept5Id,
                ProductName = "Playstation 1",
                Description = productDescription,
                Price = 1990.00m,
                Quantity = 20
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

        // Seeda Ordrar
        var order1Id = 1;

        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                OrderId = order1Id,
                CustomerId = customer1StaticId,
                UserName = "user1",
                OrderDate = new DateTime(2025, 1, 1),
            }
        );

        modelBuilder.Entity<OrderProduct>().HasData(
            new OrderProduct
            {
                Id = 1, 
                OrderId = order1Id,
                ProductId = product1Id,
                ProductQuantity = 2 
            },
            new OrderProduct
            {
                Id = 2, 
                OrderId = order1Id,
                ProductId = product2Id,
                ProductQuantity = 1 
            }
        );

        var order2Id = 2;

        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                OrderId = order2Id,
                CustomerId = customer1StaticId,
                UserName = "user1",
                OrderDate = new DateTime(2025, 1, 1),
            }
        );

        modelBuilder.Entity<OrderProduct>().HasData(
            new OrderProduct
            {
                Id = 3,
                OrderId = order2Id,
                ProductId = product4Id,
                ProductQuantity = 5
            },
            new OrderProduct
            {
                Id = 4,
                OrderId = order2Id,
                ProductId = product8Id,
                ProductQuantity = 1
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
