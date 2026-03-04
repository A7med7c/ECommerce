using ECommerce.DAL.Entities.IdentityModule;

namespace ECommerce.DAL.Entities
{
    /// <summary>
    /// Represents a user's saved/favourite product.
    /// Does NOT inherit BaseEntity because the user PK is a string (IdentityUser.Id),
    /// so the int-based CreatedBy audit field would be misleading.
    /// </summary>
    public class Favorite
    {
        public int Id { get; set; }

        /// <summary>FK → AspNetUsers.Id (string GUID).</summary>
        public string UserId { get; set; } = null!;

        public int ProductId { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // ── Navigation ────────────────────────────────────────────────────
        public ApplicationUser User { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
