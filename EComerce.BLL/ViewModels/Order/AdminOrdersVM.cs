namespace ECommerce.BLL.ViewModels.Order
{


    public class AdminOrdersVM
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingCity { get; set; } = null!;


        public string UserEmail { get; set; } = null!;
        public string UserFullName { get; set; } = null!;
    }
}
