using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechElite;
using TechElite.Areas.Identity.Data;
using TechElite.Models;

var builder = WebApplication.CreateBuilder(args);

// Hämta anslutningssträngen från konfigurationsfilen
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Lägg till Identity-tjänster och databasanslutning för SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Lägg till Identity-tjänster
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Lägg till autentiseringstjänster, inklusive Negotiate (Windows Authentication)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddCookie()  // Standard cookie-baserad autentisering
.AddNegotiate("Negotiate", options => { });  // Lägg till Negotiate för Windows Authentication

// Lägg till tjänster för Razor Pages och Controllers
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Lägg till DbContext för din kontaktinformation (om den är separat)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Seeda användare och roller (om du har en seeding-metod)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Seed användare och roller om du har en metod för det
    // await ApplicationDbContext.SeedUsersAndRolesAsync(services);
}

// Konfigurera HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();  // Lägg till autentisering
app.UseAuthorization();   // Lägg till auktorisering

app.MapStaticAssets();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();