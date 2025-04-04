using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using TechElite.Areas.Identity.Data;

namespace TechElite.Models
{
    public class Review
    {
        [Key]
        public required string ReviewId { get; set; }

        [ForeignKey("Product")]
        public string? ProductId { get; set; } // Nullable för att kunna skapa recensioner utan att ha en produkt kopplad
        public Product? Product { get; set; }  // Navigeringsegenskap

        [Required]
        [StringLength(40)]
        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        public required string ReviewerName { get; set; } = string.Empty; // Så användaren inte behöver skriva sitt anv.namn eller riktiga namn

        [Required]
        [StringLength(40)]
        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        public required string ReviewTitle { get; set; } = string.Empty;

        [Required]
        [Range(1, 5, ErrorMessage = "Du måste ge mellan 1 och 5 stjärnor.")]
        public required int Rating { get; set; }

        [Required]
        [StringLength(500)]
        public required string ReviewText { get; set; } = string.Empty;

        public DateTime ReviewDate { get; set; } = DateTime.Now;

        [ForeignKey("ApplicationUser")]
        public required string CustomUserId { get; set; } 
        public ApplicationUser? ApplicationUser { get; set; } 

    }
}
