using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechEliteWebshop.Areas.Identity.Data;

namespace TechEliteWebshop.Models
{
    public class ForumThread
    {
        [Key]
        public string ThreadId { get; set; } = GenerateThreadId();

        [Required]
        [StringLength(100)]
        public required string Title { get; set; } = string.Empty;

        [Required]
        public required string Content { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("ForumCategory")]
        public required string CategoryId { get; set; }
        public required ForumCategory Category { get; set; }

        [ForeignKey("ApplicationUser")]
        public required string CustomUserId { get; set; } 
        public required ApplicationUser ApplicationUser { get; set; }
        public required string UserName { get; set; } = string.Empty;

        public ICollection<ForumReply> Replies { get; set; } = new List<ForumReply>();

        private static string GenerateThreadId()
        {
            return $"THR-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}



