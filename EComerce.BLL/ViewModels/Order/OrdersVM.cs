namespace ECommerce.BLL.ViewModels.Order
{
    /// <summary>Lightweight row used by the Order list view.</summary>
    public class OrdersVM
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingCity { get; set; } = null!;
    }
}
