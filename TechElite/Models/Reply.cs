namespace TechElite.Models
{
    public class Reply
    {
        public int ReplyId { get; set; }
        public int ThreadId { get; set; }
        public int MemberId { get; set; }
        public DateTime ReplyDate { get; set; }
        public string? Heading { get; set; }
        public string? ReplyText { get; set; }
        public int FCategoryId { get; set; }
    }
}
