namespace ECommerce.DAL.Entities
{
    public class Address
    {
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Zip { get; set; } = null!;
        public bool IsDefault { get; set; }
        public int UserId { get; set; } //fk
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Order>  Orders { get; set; } = new HashSet<Order>();
    }
}
