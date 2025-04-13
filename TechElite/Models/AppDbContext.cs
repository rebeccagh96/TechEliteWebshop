using Microsoft.EntityFrameworkCore;
using TechElite.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserContact> UserContacts { get; set; }
}