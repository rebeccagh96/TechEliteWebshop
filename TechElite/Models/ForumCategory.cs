namespace TechElite.Models
{
    public class ForumCategory
    {
        public int ForumCategoryId { get; set; }
        public string? Category {  get; set; }
        public ICollection<Thread>? Threads { get; set; }
        public ICollection<Reply>? Replies { get; set; }
    }
}
