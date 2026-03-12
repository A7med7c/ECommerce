namespace ECommerce.BLL.ViewModels.Cart
{


    public class CartVM
    {
        public IReadOnlyList<CartItemVM> Items { get; init; } = [];

        public decimal SubTotal => Items.Sum(i => i.LineTotal);
        public int TotalItems => Items.Sum(i => i.Quantity);
        public bool IsEmpty => Items.Count == 0;
    }
}
