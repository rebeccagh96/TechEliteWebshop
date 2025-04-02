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

        // När ni har hämtat från GH, följ dessa steg för att fullt ut migrera korrekt och få databasen att fungera:
        // 1. Radera migrationsmappen, OCH databasen, om du har en gammal version av den kvar i SQL-server object explorer
        // 2. Öppna package manager console
        // 3. Skriv "Add-Migration InitialCreate" och tryck enter
        // 4. Skriv "Update-Database" och tryck enter
        // 5. Kör programmet och se att det fungerar, då seedas även användare från SeedUsers-filen
        // 6. När detta gjort och du inte fått några errors, gå in i ApplicationDbContext-filen och ta av-kommentera
        // de delar som är utkommenterade. Alltså customer-seeding, forumthread-seeding och review-seeding
        // 7. Gå sedan in i databasen, börja med att refresha den, uppe till vänster, gå sedan till:
        // TechElite>Tables>högerklicka på DboAspNetUsers>"View Data", håll den öppen på sidan om
        // 8. Gå igenom ApplicationDbContext och byt ut de UserId's som finns till några från din egen databas * viktigt *
        // jag har försökt göra det tydligt genom att markera vart med kommentarer.
        // 9. När detta är gjort, gör en ny migration, och uppdatera databasen igen
        // 10. Refresha databasen, och kolla att det gått korrekt genom att öppna t.ex. ForumThreads och se att det finns data

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

            // Flyttat användarseeding till egen fil då det verkar vara problematiskt att seeda användare med lösenordshashing på detta sätt, skapar ny hash varje gång

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    UserId = "6d57f11a-0f49-4c6f-bb67-ee7b04cfe80f", // Byt ut denna userId mot en som finns i din databas
                    FirstName = "User1",
                    LastName = "Userson",
                    Address = "User street 1",
                    ZipCode = "12345",
                    City = "User city"
                },
                new Customer
                {
                    CustomerId = 2,
                    UserId = "80ad6ff6-2f2a-432c-8437-d53f2f821df5", // Byt ut denna userId mot en som finns i din databas
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

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    ProductName = "Laptop, 16 tum",
                    ProductDescription = "16-tums bärbar dator",
                    ImagePath = "img/Products/Laptop/Laptop1.svg",
                    ProductDepartmentId = 1,
                    Stock = 10,
                    Price = 5990
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "Laptop, 12 tum",
                    ProductDescription = "12-tums bärbar dator",
                    ImagePath = "img/Products/Laptop/Laptop2.svg",
                    ProductDepartmentId = 1,
                    Stock = 10,
                    Price = 3990
                },
                new Product
                {
                    ProductId = 3,
                    ProductName = "iPhone 5, 128gb",
                    ProductDescription = "En iPhone 5",
                    ImagePath = "img/Products/Phone/Phone1.svg",
                    ProductDepartmentId = 2,
                    Stock = 10,
                    Price = 1990
                },
                new Product
                {
                    ProductId = 4,
                    ProductName = "Samsung Galaxy",
                    ProductDescription = "En Samsung Galaxy",
                    ImagePath = "img/Products/Phone/Phone4.svg",
                    ProductDepartmentId = 2,
                    Stock = 10,
                    Price = 2990
                },
                new Product
                {
                    ProductId = 5,
                    ProductName = "Noice cancelling headset",
                    ProductDescription = "Ett noice cancelling headset",
                    ImagePath = "img/Products/Headphones/Headphones1.svg",
                    ProductDepartmentId = 3,
                    Stock = 10,
                    Price = 2990
                },
                new Product
                {
                    ProductId = 6,
                    ProductName = "Samsung Galaxy Buds",
                    ProductDescription = "Trådlösa in-ear-hörlurar",
                    ImagePath = "img/Products/Earbuds/Buds2.svg",
                    ProductDepartmentId = 3,
                    Stock = 10,
                    Price = 2490
                },
                new Product
                {
                    ProductId = 7,
                    ProductName = "Extern hårddisk, 3tb",
                    ProductDescription = "En extern hårddisk på 3 terrabyte",
                    ImagePath = "img/Products/Components/Ext-hdd1.svg",
                    ProductDepartmentId = 4,
                    Stock = 10,
                    Price = 1490
                },
                new Product
                {
                    ProductId = 8,
                    ProductName = "Grafikkort",
                    ProductDescription = "Ett grafikkort",
                    ImagePath = "img/Products/Components/GPU1.svg",
                    ProductDepartmentId = 4,
                    Stock = 10,
                    Price = 4490
                },
                new Product
                {
                    ProductId = 9,
                    ProductName = "Sony Playstation",
                    ProductDescription = "Ett klassiskt Playstation",
                    ImagePath = "img/Products/Consoles/Console2.svg",
                    ProductDepartmentId = 5,
                    Stock = 10,
                    Price = 2490
                },
                new Product
                {
                    ProductId = 10,
                    ProductName = "Dualshock handkontroll",
                    ProductDescription = "En handkontroll",
                    ImagePath = "img/Products/Controllers/Controller1.svg",
                    ProductDepartmentId = 5,
                    Stock = 10,
                    Price = 490
                }
            );

            // Orders kommer att seedas vid behov

            modelBuilder.Entity<ForumCategory>().HasData(
                new ForumCategory
                {
                    ForumCategoryId = 1,
                    CategoryName = "Produkter",
                    CategoryDescription = "Diskutera produkter",
                },
                new ForumCategory
                {
                    ForumCategoryId = 2,
                    CategoryName = "Support",
                    CategoryDescription = "Få hjälp med produkter",
                },
                new ForumCategory
                {
                    ForumCategoryId = 3,
                    CategoryName = "Rekommendationer",
                    CategoryDescription = "Rekommenderationer",
                },
                new ForumCategory
                {
                    ForumCategoryId = 4,
                    CategoryName = "Tips",
                    CategoryDescription = "Tips & tricks",
                },
                new ForumCategory
                {
                    ForumCategoryId = 5,
                    CategoryName = "Köp & sälj",
                    CategoryDescription = "Köp och sälj dina gamla teknikprylar",
                },
                new ForumCategory
                {
                    ForumCategoryId = 6,
                    CategoryName = "Övrigt",
                    CategoryDescription = "Övriga diskussioner",
                }
            );

            modelBuilder.Entity<ForumThread>().HasData(
                new ForumThread
                {
                    ForumThreadId = 1,
                    ThreadTitle = "Bästa datorn?",
                    ThreadContent = "Vilken dator är bäst?",
                    PublishDate = new DateTime(2024, 03, 31, 12, 00, 00), //Ändrartillhårdkodat för att inte konstant ändra den seedade datan
                    UserId = "80ad6ff6-2f2a-432c-8437-d53f2f821df5", // Byt ut denna userId mot en som finns i din databas
                    ForumCategoryId = 1
                },
                new ForumThread
                {
                    ForumThreadId = 2,
                    ThreadTitle = "Hjälp med iPhone",
                    ThreadContent = "Min iPhone fungerar inte",
                    PublishDate = new DateTime(2024, 03, 31, 12, 00, 00),
                    UserId = "d6011b66-39ae-4dcb-9fac-c0dff7e5bbbd", // Byt ut denna userId mot en som finns i din databas
                    ForumCategoryId = 2
                }
            );

            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    ReviewId = 1,
                    ProductId = 1,
                    UserId = "d6011b66-39ae-4dcb-9fac-c0dff7e5bbbd", // Byt ut denna userId mot en som finns i din databas
                    ReviewContent = "Bra dator!",
                    Rating = 5
                },
                new Review
                {
                    ReviewId = 2,
                    ProductId = 2,
                    UserId = "d967b630-a3e2-4a08-9762-0f74293a4591", // Byt ut denna userId mot en som finns i din databas
                    ReviewContent = "Dålig dator!",
                    Rating = 1
                }
            );
        }
    }
}
