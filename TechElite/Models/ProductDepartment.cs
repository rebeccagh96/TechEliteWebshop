namespace TechElite.Models
{
    public class ProductDepartment
    {
        public int ProductDepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public ICollection<Product>? Products { get; set; }

    }
}
