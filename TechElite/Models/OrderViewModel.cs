namespace TechElite.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalPrice { get; set; }
        public List<ProductViewModel> Products { get; set; } = new();
    }
}
