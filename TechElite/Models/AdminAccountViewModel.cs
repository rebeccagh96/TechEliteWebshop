using System.Collections.Generic;
using TechElite.Models;

namespace TechElite.Models
{
    public class AdminAccountViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<OrderViewModel> Orders { get; set; }
        public IEnumerable<OrderProductViewModel> OrderProducts { get; set; }
        public IEnumerable<Cart> Carts { get; set; } = new List<Cart>();
        public IEnumerable<UserContact> UserContacts { get; set; }

        public int SelectedDepartmentId { get; set; }
    }
}