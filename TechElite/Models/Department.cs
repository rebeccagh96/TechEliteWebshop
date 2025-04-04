using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechElite.Models
{
    public class Department
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Required] // Denna annotation gör fältet obligatoriskt ifrån vyn, inte bara i DB
        [StringLength(40)]
        [RegularExpression("[A-Za-zÅÄÖåäö.'´-]+")]
        public required string DepartmentName { get; set; } = string.Empty;
        public string? DepartmentDescription { get; set; }
        public ICollection<Product>? Products { get; set; } = new List<Product>(); // Lista av produkter kopplade, nullable för att kunna skapa departments utan produkter

    }
}
