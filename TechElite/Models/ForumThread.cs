namespace TechElite.Models
{
    public class ForumThread
    {
        public int ForumThreadId { get; set; }
        public required string ThreadName { get; set; }
        public required string ThreadContent { get; set; }
        public int ForumCategoryId { get; set; }
        public required string UserId { get; set; }
        public DateTime PublishDate { get; set; }
        public ForumCategory ForumCategory { get; set; } = default!; // Saknade navigation properties
        public ApplicationUser User { get; set; } = default!;
        public ICollection<Reply>? Replies { get; set; } 


    }
}
