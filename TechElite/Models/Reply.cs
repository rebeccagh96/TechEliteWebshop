namespace TechElite.Models
{
    public class Reply
    {
        public string ReplyId { get; set; }
        public string ThreadId { get; set; }
        public string MemberId { get; set; }
        public DateTime ReplyDate { get; set; }
        public string Heading { get; set; }
        public string ReplyText { get; set; }
        public string FCategoryId { get; set; }
    }
}
