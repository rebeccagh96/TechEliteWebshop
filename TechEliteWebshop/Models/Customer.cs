using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechEliteWebshop.Areas.Identity.Data;

namespace TechEliteWebshop.Models
{
    public class Customer
    {
        [Key]
        [Column("CustomerId")]
        public string CustomerId { get; set; } = GenerateCustomerId();

        [Required] // Denna annotation gör fältet obligatoriskt ifrån vyn, inte bara i DB
        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        [StringLength(30)]
        public required string FirstName { get; set; } 

        [Required] 
        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        [StringLength(30)]
        public required string LastName { get; set; } 

        [Required]
        [StringLength(50)]
        public required string Address { get; set; }

        [Required]
        [StringLength(5)]
        [RegularExpression(@"\d{5}")]
        public required string ZipCode { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        public required string City { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>(); // Lista av ordrar kopplade till användaren

        [ForeignKey("ApplicationUser")]
        public required string CustomUserId { get; set; }
        public required ApplicationUser ApplicationUser { get; set; }
        public required string UserName { get; set; } = string.Empty;

        private static string GenerateCustomerId()
        {
            return $"CUST-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

    }
}
