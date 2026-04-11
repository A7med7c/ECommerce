using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL.Entities.IdentityModule
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        public ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
    }
}
