using System.ComponentModel.DataAnnotations;

namespace TechElite.Models
{
    public class UserContact
    {
        [Key]
        public int UserContactId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}