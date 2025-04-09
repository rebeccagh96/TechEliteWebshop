namespace TechElite.Models
{
    public record SearchViewModel
    (
        ICollection<Department> Departments,
        ICollection<Product> Products,
        ICollection<Review> Reviews
    );
}
