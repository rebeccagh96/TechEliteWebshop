namespace TechElite.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set;  } 
        public int ProductId { get; set; }
        public int ProductDepartmentId { get; set; } 
        public required string ReviewContent { get; set; }
        public int Stars {  get; set; }

        public Product Product { get; set; } = default!;
    }
}
