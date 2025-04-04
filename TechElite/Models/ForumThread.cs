using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechElite.Areas.Identity.Data;

namespace TechElite.Models
{
    public class ForumThread
    {
        [Key]
        public required string ThreadId { get; set; }

        [Required]
        [StringLength(100)]
        public required string ThreadTitle { get; set; } = string.Empty;

        [Required]
        public required string ThreadContent { get; set; } = string.Empty;

        public DateTime PublishDate{ get; set; } = DateTime.Now;

        [ForeignKey("ForumCategory")]
        public required string CategoryId { get; set; }
        public ForumCategory? Category { get; set; }

        [ForeignKey("ApplicationUser")]
        public required string CustomUserId { get; set; } 
        public ApplicationUser? ApplicationUser { get; set; }
        public required string UserName { get; set; } = string.Empty;

        public ICollection<ForumReply> Replies { get; set; } = new List<ForumReply>();

    }
}



