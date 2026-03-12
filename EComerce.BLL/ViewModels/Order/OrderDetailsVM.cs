namespace ECommerce.BLL.ViewModels.Order
{

    public class OrderDetailsVM
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int StatusValue { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }


        public string ShippingCountry { get; set; } = null!;
        public string ShippingCity { get; set; } = null!;
        public string ShippingStreet { get; set; } = null!;
        public string ShippingZip { get; set; } = null!;


        public IEnumerable<OrderItemVM> Items { get; set; } = new List<OrderItemVM>();
    }
}
