namespace TechElite.Models
{
    public class Review
    {
        public string ReviewId { get; set; }
        public string UserId { get; set;  } 

        public int ProductId { get; set; }
        public string ReviewText { get; set; }

        public int Stars {  get; set; }
    }
}
