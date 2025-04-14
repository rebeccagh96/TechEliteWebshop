namespace TechElite.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        
        public IEnumerable<OrderProductViewModel> Products { get; set; } = new List<OrderProductViewModel>();

    }
}
