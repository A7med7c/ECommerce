namespace ECommerce.BLL.ViewModels.Favorite
{
    /// <summary>Represents a single item on the My Favourites page.</summary>
    public class FavoriteVM
    {
        public int FavoriteId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductImageUrl { get; set; }
        public decimal ProductPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; } = null!;
        public DateTime AddedOn { get; set; }
    }
}
