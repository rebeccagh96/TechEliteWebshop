using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechElite.Areas.Identity.Data;

namespace TechElite.Models
{
    public class Customer
    {
        [Key]
        [Column("CustomerId")]
        public required string CustomerId { get; set; }

        [Required] // Denna annotation gör fältet obligatoriskt ifrån vyn, inte bara i DB
        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        [StringLength(30)]
        public required string FirstName { get; set; } 

        [Required] 
        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        [StringLength(30)]
        public required string LastName { get; set; } 

        [Required]
        [StringLength(50)]// regex
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
        public string? CustomUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string? UserName { get; set; } = string.Empty;


    }
}
