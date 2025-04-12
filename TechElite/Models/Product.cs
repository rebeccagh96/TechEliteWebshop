using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechElite.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; } 
        public Department? Department { get; set; } 

        [Required] // Denna annotation gör fältet obligatoriskt ifrån vyn, inte bara i DB
        [StringLength(40)]
        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        public required string ProductName { get; set; }

        [Required] 
        [StringLength(150)]
        public required string Description { get; set; }

        [Required]
        public required int Price { get; set; }
        
        [Column(TypeName = "image")]
        public byte[]? Image { get; set; } // Nullable för att kunna ha produkter utan bild (Främst för testning)

        [Required] 
        public required int Quantity { get; set; } // Antal i lager

        public ICollection<Review>? Reviews { get; set; } = new List<Review>();
        public ICollection<OrderProduct>? OrderProducts { get; set; } = new List<OrderProduct>(); // Lista av orderprodukter kopplade, nullable för att kunna skapa produkter utan orderprodukter

    }
}