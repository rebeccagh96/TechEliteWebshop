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

        //[RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        //[StringLength(40)]
        //public string ProductName { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>(); // Lista av produkter kopplade till ordern

        [NotMapped]
        public int TotalPrice => Products.Sum(p => p.Price * p.Quantity);

    }
}