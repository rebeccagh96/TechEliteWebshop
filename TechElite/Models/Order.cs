using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechElite.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; } 
        public Customer? Customer { get; set; }
        public required string UserName { get; set; } = string.Empty; // Användarnamn kopplat till ordern
        public required DateTime OrderDate { get; set; }

        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        [StringLength(40)]
        public required string ProductName { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>(); // Lista av produkter kopplade till ordern
        public int TotalPrice { get; set; } // Totalpris för ordern

    }
}