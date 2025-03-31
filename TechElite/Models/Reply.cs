namespace TechElite.Models
{
    public class Reply
    {
        public int ReplyId { get; set; }
        public int ThreadId { get; set; }
        public required string UserId { get; set; }
        public DateTime ReplyDate { get; set; }
        public required string ReplyContent { get; set; }
        public int ForumCategoryId { get; set; }
        public ApplicationUser User { get; set; } = default!; // Både denna och nästa rad var inkorrekt skrivna
        public ForumCategory ForumCategory { get; set; } = default!;
        public ForumThread Thread { get; set; } = default!; // Denna saknades
    }
}
