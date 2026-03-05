namespace ECommerce.BLL.ViewModels.Order
{
    /// <summary>
    /// Admin order list row — includes customer identity on top of the
    /// standard customer-facing OrdersVM.
    /// </summary>
    public class AdminOrdersVM
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingCity { get; set; } = null!;

        // Customer info
        public string UserEmail { get; set; } = null!;
        public string UserFullName { get; set; } = null!;
    }
}
