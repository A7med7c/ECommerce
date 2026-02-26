namespace ECommerce.DAL.Entities
{
    public class Product : BaseEntity 
    {
        public string Name { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; } // FK
        // Navigation
        public Category Category { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>()
    }
}
