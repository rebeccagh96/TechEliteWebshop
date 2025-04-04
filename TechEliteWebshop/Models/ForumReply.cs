using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechEliteWebshop.Areas.Identity.Data;

namespace TechEliteWebshop.Models
{
    public class ForumReply
    {
        [Key]
        public string ReplyId { get; set; } = GenerateReplyId();

        [Required]
        public required string Content { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("ForumThread")]
        public required string ThreadId { get; set; }
        public required ForumThread Thread { get; set; }

        [ForeignKey("ApplicationUser")]
        public required string CustomUserId { get; set; } 
        public required ApplicationUser ApplicationUser { get; set; } 
        public required string UserName { get; set; } = string.Empty;

        private static string GenerateReplyId()
        {
            return $"REP-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}



