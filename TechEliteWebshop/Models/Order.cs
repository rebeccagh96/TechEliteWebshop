using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechEliteWebshop.Models
{
    public class Order
    {
        [Key]
        [Column("OrderId")]
        public string OrderId { get; set; } = GenerateOrderId();

        [ForeignKey("Customer")]
        public required string CustomerId { get; set; } 
        public required Customer Customer { get; set; } 
        public required string UserName { get; set; } = string.Empty; // Användarnamn kopplat till ordern
        public required DateTime OrderDate { get; set; }
        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        [StringLength(40)]
        public required string ProductName { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>(); // Lista av produkter kopplade till ordern
        public int TotalPrice { get; set; } // Totalpris för ordern
        private static string GenerateOrderId()
        {
            return $"ORD-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

    }
}