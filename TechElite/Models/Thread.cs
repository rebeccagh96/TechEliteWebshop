namespace TechElite.Models
{
    public class Thread
    {
        public string ThreadId { get; set; }
        public string FCategoryId { get; set; }
        public string MemberId { get; set; }
        public DateTime PublishDate { get; set; }
        public string ThreadReplyText { get; set; }
    }
}
