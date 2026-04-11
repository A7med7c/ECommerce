using ECommerce.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EComerce.DAL.Data.Configurations
{
    internal class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Id).UseIdentityColumn(1, 1);


            builder.HasIndex(f => new { f.UserId, f.ProductId }).IsUnique();

            builder.Property(f => f.CreatedAt )
                   .HasDefaultValueSql("GETUTCDATE()");


            builder.HasOne(f => f.User)
                   .WithMany(u => u.Favorites)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(f => f.Product)
                   .WithMany(p => p.Favorites)
                   .HasForeignKey(f => f.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
