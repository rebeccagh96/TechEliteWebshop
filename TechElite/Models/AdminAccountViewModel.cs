using System.Collections.Generic;
using TechElite.Models;

namespace TechElite.Models
{
    public class AdminAccountViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<OrderViewModel> OrderViewModels { get; set; }
        public int SelectedDepartmentId { get; set; }
    }
}