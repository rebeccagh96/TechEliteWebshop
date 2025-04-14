namespace TechElite.Models
{
    public class CartPageViewModel
    {
        public List<OrderProductViewModel> CartItems { get; set; } = new();

        public Customer Customer { get; set; } = new Customer
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            Address = string.Empty,
            ZipCode = string.Empty,
            City = string.Empty,
            UserName = string.Empty
        };
    }
}
