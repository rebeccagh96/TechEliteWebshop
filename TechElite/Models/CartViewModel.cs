namespace TechElite.Models
{
    public class CartViewModel
    {
        public List<OrderProductViewModel> OrderProducts { get; set; } = new List<OrderProductViewModel>();

        public List<Order> Orders { get; set; } = new List<Order>();
        public decimal TotalPrice { get; set; }
        

    }

}
