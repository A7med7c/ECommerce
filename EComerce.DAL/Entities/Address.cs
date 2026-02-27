using ECommerce.DAL.Entities.IdentityModule;

namespace ECommerce.DAL.Entities
{
    public class Address : BaseEntity
    {
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Zip { get; set; } = null!;
        public bool IsDefault { get; set; }
        public string UserId { get; set; } = null!;//fk
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Order>  Orders { get; set; } = new HashSet<Order>();
    }
}
