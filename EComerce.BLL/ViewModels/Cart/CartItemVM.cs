namespace ECommerce.BLL.ViewModels.Cart
{


    public class CartItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }


        public int StockQuantity { get; set; }


        public decimal LineTotal => UnitPrice * Quantity;
    }
}
