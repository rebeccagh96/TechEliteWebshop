namespace TechElite.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderProductViewModel> OrderProducts { get; set; } = new List<OrderProductViewModel>(); // Lista av produkter kopplade till ordern
        public decimal TotalPrice { get; set; } // Totalpris för ordern
    }
}
