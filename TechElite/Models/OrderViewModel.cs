namespace TechElite.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; } // Kopplad till kund
        public string? UserName { get; set; } // Användarnamn kopplat till ordern
        public DateTime OrderDate { get; set; } // Datum för ordern
        public int ProductId { get; set; } // Kopplad till produkt
        public string? ProductName { get; set; } // Namn på produkten
        public int ProductUnits { get; set; } // Antal enskilda produkter i ordern
        public int UnitPrice { get; set; } // Pris för produkten
        public int TotalPrice { get; set; } // Totalpris för ordern
    }
}
