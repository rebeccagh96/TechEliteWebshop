namespace TechElite.Models
{
    public record ShopViewModel
     (   
        ICollection<Department> Departments,
        ICollection<Product> Products,
        ICollection<Review> Reviews
        
     );
}
