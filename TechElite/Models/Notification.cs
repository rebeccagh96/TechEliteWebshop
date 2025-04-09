using TechElite.Areas.Identity.Data;

namespace TechElite.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public int ThreadId { get; set; }
        public ForumThread ForumThread { get; set; }

    }
}
