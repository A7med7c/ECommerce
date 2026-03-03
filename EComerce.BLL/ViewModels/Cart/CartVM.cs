namespace ECommerce.BLL.ViewModels.Cart
{
    /// <summary>
    /// Full shopping-cart view model passed to the Cart/Index view.
    /// All aggregates are computed properties — zero extra allocations.
    /// </summary>
    public class CartVM
    {
        public IReadOnlyList<CartItemVM> Items { get; init; } = [];

        public decimal SubTotal => Items.Sum(i => i.LineTotal);
        public int TotalItems => Items.Sum(i => i.Quantity);
        public bool IsEmpty => Items.Count == 0;
    }
}
