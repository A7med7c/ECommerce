namespace ECommerce.DAL.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } = null!;
        public int Status { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int AddressId { get; set; } //fk
        public int UserId { get; set; } //fk
        public ApplicationUser User { get; set; }
        public Address Address { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

    }
}
