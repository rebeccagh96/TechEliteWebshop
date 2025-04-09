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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReplyId { get; set; }

        [Required]
        public required string Content { get; set; } = string.Empty;

        public DateTime PublishDate { get; set; } = DateTime.Now;

        [ForeignKey("ForumThread")]
        public int ThreadId { get; set; }
        public ForumThread? Thread { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public required string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string? UserName { get; set; } = string.Empty;

    }
}



