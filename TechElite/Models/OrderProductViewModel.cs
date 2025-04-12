namespace TechElite.Models
{
    public class OrderProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int ProductQuantity { get; set; } // Antal av produkten i ordern
    }
}
