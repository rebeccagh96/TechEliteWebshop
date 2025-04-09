using System.Collections.Generic;
using TechElite.Models;

namespace TechElite.Models
{
    public class AdminAccountViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}