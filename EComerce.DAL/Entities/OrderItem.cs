namespace ECommerce.DAL.Entities
{
    public class OrderItem : BaseEntity 
    {
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }
}
