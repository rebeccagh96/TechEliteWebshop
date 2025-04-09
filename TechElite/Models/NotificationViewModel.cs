using System.ComponentModel.DataAnnotations.Schema;
using TechElite.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace TechElite.Models
{
    public class NotificationViewModel
    {
        public int NotificationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public int ForumReplyId { get; set; }
        public ForumReply ForumReply { get; set; }
    }
}
