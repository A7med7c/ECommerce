using ECommerce.DAL.Entities;
using ECommerce.DAL.Entities.IdentityModule;

namespace EComerce.DAL.Data.Configurations
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // PK
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).UseIdentityColumn(1, 1);

            // Properties
            builder.Property(o => o.OrderNumber)
                   .HasColumnType("varchar(50)")
                   .IsRequired();

            builder.HasIndex(o => o.OrderNumber).IsUnique();

            builder.Property(o => o.Status).IsRequired();

            builder.Property(o => o.OrderDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(o => o.TotalAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(o => o.UserId).IsRequired();

            // Relationship: Customer 1→∞ Order
            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Order ∞→1 Address (shipping)
            builder.HasOne(o => o.ShippingAddress)
                   .WithMany(a => a.Orders)
                   .HasForeignKey(o => o.ShippingAddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Audit columns
            builder.Property(o => o.CreatedOn).HasDefaultValueSql("GETDATE()");
            builder.Property(o => o.ModifiedOn).HasDefaultValueSql("GETDATE()");
            builder.Property(o => o.IsDeleted).HasDefaultValue(false);
        }
    }
}
