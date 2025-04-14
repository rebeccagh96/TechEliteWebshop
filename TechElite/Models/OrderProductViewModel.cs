using System.ComponentModel.DataAnnotations;

namespace TechElite.Models
{
    public class OrderProductViewModel
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int ProductQuantity { get; set; } // Antal av produkten i ordern

        public int CartQuantity { get; set; } // Antal av produkten i kundvagnen

       
    }
}
