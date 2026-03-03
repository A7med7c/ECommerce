namespace ECommerce.BLL.ViewModels.Cart
{
    /// <summary>
    /// Represents a single product line stored in the session cart.
    /// StockQuantity is snapshotted at add-time to validate quantity
    /// updates without extra DB round-trips.
    /// </summary>
    public class CartItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        /// <summary>
        /// DB stock snapshotted when the item was first added.
        /// Used by UpdateQuantity to enforce the stock ceiling
        /// without an extra DB hit on every slider/input change.
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>Computed line total; not stored in session — derived on read.</summary>
        public decimal LineTotal => UnitPrice * Quantity;
    }
}
