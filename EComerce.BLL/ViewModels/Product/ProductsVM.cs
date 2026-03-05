namespace ECommerce.BLL.ViewModels.Product
{
    public class ProductsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
