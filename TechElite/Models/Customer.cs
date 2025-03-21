namespace TechElite.Models
{
    public class Customer
    {
        public int OrderID { get; set; }

        public int UserID { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? ZipCode { get; set; }

        public string? City { get; set; }
    }
}
