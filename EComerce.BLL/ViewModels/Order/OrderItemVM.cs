namespace ECommerce.BLL.ViewModels.Order
{
    /// <summary>Single line item embedded inside OrderDetailsVM.</summary>
    public class OrderItemVM
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
