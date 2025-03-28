namespace TechElite.Models
{
    public class Reply
    {
        public int ReplyId { get; set; }
        public int ThreadId { get; set; }
        public int UserId { get; set; }
        public DateTime ReplyDate { get; set; }
        public required string ReplyContent { get; set; }
        public int ForumCategoryId { get; set; }
        public required User User { get; set; }
        public required ForumCategory ForumCategory { get; set; }
    }
}
