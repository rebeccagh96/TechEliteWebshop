using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechEliteWebshop.Models
{
    public class ForumCategory
    {
        [Key]
        public string CategoryId { get; set; } = GenerateCategoryId();

        [Required]
        [StringLength(50)]
        public required string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        public ICollection<ForumThread> Threads { get; set; } = new List<ForumThread>();

        private static string GenerateCategoryId()
        {
            return $"CAT-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}



