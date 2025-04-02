namespace TechElite.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public required string UserId { get; set;  } 
        public int ProductId { get; set; }
        public required string ReviewContent { get; set; }
        public int Rating {  get; set; }
        public Product Product { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!; // Saknad nav prop
    }
}
