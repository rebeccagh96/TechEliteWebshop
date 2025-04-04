using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechElite.Areas.Identity.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TechElite.Models
{
    public class ForumReply
    {
        [Key]
        public required string ReplyId { get; set; }

        [Required]
        public required string Content { get; set; } = string.Empty;

        public DateTime PublishDate { get; set; } = DateTime.Now;

        [ForeignKey("ForumThread")]
        public string? ThreadId { get; set; }
        public ForumThread? Thread { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? CustomUserId { get; set; } 
        public ApplicationUser? ApplicationUser { get; set; } 
        public string? UserName { get; set; } = string.Empty;

    }
}



