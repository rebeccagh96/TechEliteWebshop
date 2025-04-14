namespace TechElite.Models
{
    public class OrderEditDto
    {
        public int OrderId { get; set; }
        public List<OrderProductEditDto> OrderProducts { get; set; } = new List<OrderProductEditDto>();
    }

    public class OrderProductEditDto
    {
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }

}
