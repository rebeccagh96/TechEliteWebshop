using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TechElite.Models;
using TechElite.Areas.Identity;

namespace TechElite.Areas.Identity.Data;

public class ApplicationUser : IdentityUser
{
    // IdentityUser inkluderar följande egenskaper:
    // *Id, *UserName, NormalizedUserName, *Email, NormalizedEmail, *EmailConfirmed,
    // *PasswordHash, SecurityStamp, ConcurrencyStamp, *PhoneNumber, PhoneNumberConfirmed,
    // TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount
    // Stjärnmarkerade är de vi kommer att använda i denna applikation, så långt jag kan avgöra just nu i alla fall

    [Required] // Denna annotation gör fältet obligatoriskt ifrån vyn, inte bara i DB
    [StringLength(30)]
    [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
    [Display(Name = "Förnamn")] 
    public required string FirstName { get; set; } 

    [Required]
    [StringLength(30)]
    [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
    [Display(Name = "Efternamn")]
    public required string LastName { get; set; }
    public Customer? Customer { get; set; } // Navigeringsegenskap för att koppla till Customer-modellen, OM användaren även är customer kan man tack vare detta navigera till customertabellen


}