using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechElite.Models
{
    public class ForumCategory
    {
        [Key]
        public required string CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public required string CategoryName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        public ICollection<ForumThread> Threads { get; set; } = new List<ForumThread>();

    }
}



