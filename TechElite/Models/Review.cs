namespace TechElite.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set;  } 

        public int ProductId { get; set; }
        public string ReviewText { get; set; }

        public int Stars {  get; set; }
    }
}
