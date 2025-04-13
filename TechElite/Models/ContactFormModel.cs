using System.ComponentModel.DataAnnotations;

namespace TechElite.Models
{
    public class ContactFormModel
    {
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