using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TechElite.Models;

namespace TechElite.Data
{
    public class SeedUsers
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {


            // Hämtar RoleManager och UserManager från DI-containern
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Skapar en array med roller
            var roles = new[] { "Admin", "User" };

            // Loopar igenom arrayen och skapar roller om de inte finns
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var admins = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "Admin1", Email = "admin1@techelite.com", EmailConfirmed = true },
                new ApplicationUser { UserName = "Admin2", Email = "admin2@techelite.com", EmailConfirmed = true },
                new ApplicationUser { UserName = "Admin3", Email = "admin3@techelite.com", EmailConfirmed = true }
            };

            foreach (var newAdmin in admins)
            {
                var existingUser = await userManager.FindByEmailAsync(newAdmin.Email);
                if (existingUser == null)
                {
                    var result = await userManager.CreateAsync(newAdmin, "User123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newAdmin, "Admin");
                        Console.WriteLine($"Skapade användare: {newAdmin.UserName} med ID: {newAdmin.Id}");
                    }
                    else
                    {
                        throw new Exception($"Kunde inte skapa {newAdmin.UserName}: {string.Join(", ", result.Errors)}");
                    }
                }

            }

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "User1", Email = "user1@techelite.com", EmailConfirmed = true },
                new ApplicationUser { UserName = "User2", Email = "user2@techelite.com", EmailConfirmed = true },
                new ApplicationUser { UserName = "User3", Email = "user3@techelite.com", EmailConfirmed = true }
            };

            foreach (var newUser in users)
            {
                var existingUser = await userManager.FindByEmailAsync(newUser.Email);
                if (existingUser == null)
                {
                    var result = await userManager.CreateAsync(newUser, "User123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, "User");
                        Console.WriteLine($"Skapade användare: {newUser.UserName} med ID: {newUser.Id}");
                    }
                    else
                    {
                        throw new Exception($"Kunde inte skapa {newUser.UserName}: {string.Join(", ", result.Errors)}");
                    }
                }

            }
        }
    }
}
