using Microsoft.AspNetCore.Identity;
using System.Collections;
using TechElite.Areas.Identity.Data;
using System.Collections.Generic;
using TechElite.Models;
using System.ComponentModel.DataAnnotations;

namespace TechElite.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Användarnamn")]
        public string? UserName { get; set; }

        [Display(Name = "Roll")]
        public IList<string>? Roles { get; set; }

        [Display(Name = "E-post")]
        public string? Email { get; set; }

        [Display(Name = "Förnamn")]
        public string? FirstName { get; set; }

        [Display(Name = "Efternamn")]
        public string? LastName { get; set; }



    }
}
