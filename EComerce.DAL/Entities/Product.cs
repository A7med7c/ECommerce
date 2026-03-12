namespace ECommerce.DAL.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
    }
}
