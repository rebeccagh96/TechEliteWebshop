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
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>(); // Lista av produkter kopplade till ordern
        public decimal TotalPrice => OrderProducts.Sum(op => op.Product.Price * op.ProductQuantity);


    }
}