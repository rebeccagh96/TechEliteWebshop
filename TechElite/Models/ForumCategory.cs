namespace TechElite.Models
{
    public class ForumCategory
    {
        public int ForumCategoryId { get; set; }
        public string? CategoryName {  get; set; }
        public string? CategoryDescription { get; set; }
        public ICollection<ForumThread>? Threads { get; set; }
        public ICollection<Reply>? Replies { get; set; }
    }
}
