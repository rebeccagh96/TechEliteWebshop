namespace TechElite.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public byte[] Image { get; set; } = Array.Empty<byte>();
        public int ProductDepartmentId { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ProductDepartment ProductDepartment { get; set; } = default!; // Saknade navigation property här med

    }
}
