namespace TechElite.Models
{
    public class ProductDepartment
    {
        public int ProductDepartmentId { get; set; }
        public string DepartmentName { get; set; } = default!;
        public string? Department {  get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
