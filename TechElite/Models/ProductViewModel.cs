using System.ComponentModel.DataAnnotations;

namespace TechElite.Models
{
    public class ProductViewModel
    {
        public int? ProductId { get; set; }

        [Display(Name = "Produktnamn")]
        [StringLength(40)]
        public required string ProductName { get; set; }

        [Display(Name = "Saldo")]
        public int Quantity { get; set; }

        [Display(Name = "Pris")]
        public int Price { get; set; }

        [Display(Name = "Beskrivning")]
        public required string Description { get; set; }

        [Display(Name = "Avdelning")]
        public required int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        [Display(Name = "Bild")]
        public byte[]? Image { get; set; }

        [Display(Name = "Recensioner")]
        public ICollection<Review>? Reviews { get; set; } = new List<Review>();

    }
}
