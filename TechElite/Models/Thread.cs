namespace TechElite.Models
{
    public class Thread
    {
        public int ThreadId { get; set; }
        public required string ThreadName { get; set; }
        public required string ThreadContent { get; set; }
        public int ForumCategoryId { get; set; }
        public int UserId { get; set; }
        public DateTime PublishDate { get; set; }


    }
}
