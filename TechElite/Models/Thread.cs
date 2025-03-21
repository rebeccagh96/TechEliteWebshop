namespace TechElite.Models
{
    public class Thread
    {
        public int ThreadId { get; set; }
        public int FCategoryId { get; set; }
        public int MemberId { get; set; }
        public DateTime PublishDate { get; set; }
        public required string ThreadReplyText { get; set; }
    }
}
