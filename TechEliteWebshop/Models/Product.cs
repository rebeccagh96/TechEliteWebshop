using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechEliteWebshop.Models
{
    public class Product
    {
        [Key]
        [Column("ProductId")]
        public string ProductId { get; set; } = GenerateProductId();
        [ForeignKey("Department")]
        public required string DepartmentId { get; set; } 
        public required Department Department { get; set; } 

        [Required] // Denna annotation gör fältet obligatoriskt ifrån vyn, inte bara i DB
        [StringLength(40)]
        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        public required string ProductName { get; set; } = string.Empty;

        [Required] 
        [StringLength(150)]
        public required string Description { get; set; } = string.Empty;

        [Required]
        public required int Price { get; set; }
        
        [Column(TypeName = "image")]
        public byte[]? Image { get; set; } // Nullable för att kunna ha produkter utan bild

        [Required] 
        public required int Quantity { get; set; } // Antal i lager

        public ICollection<Review>? Reviews { get; set; } = new List<Review>(); 
        public ICollection<Order>? Orders { get; set; } = new List<Order>(); // För att visa antal ordrar

        public static string GenerateProductId()
        {
            // Generera ett unikt ProductId
            return $"PROD-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}