namespace ECommerce.DAL.Entities
{
    public class ApplicationUser
    {
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
