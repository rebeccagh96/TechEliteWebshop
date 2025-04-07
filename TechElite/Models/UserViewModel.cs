using Microsoft.AspNetCore.Identity;
using System.Collections;
using TechElite.Areas.Identity.Data;
using System.Collections.Generic;
using TechElite.Models;

namespace TechElite.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<string> Roles { get; set; }
    }
}
