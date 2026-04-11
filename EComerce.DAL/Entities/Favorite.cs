using ECommerce.DAL.Entities.IdentityModule;

namespace ECommerce.DAL.Entities
{


    public class Favorite
    {
        public int Id { get; set; }


        public string UserId { get; set; } = null!;

        public int ProductId { get; set; }

        public DateTime CreatedAt  { get; set; } = DateTime.UtcNow;


        public ApplicationUser User { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
