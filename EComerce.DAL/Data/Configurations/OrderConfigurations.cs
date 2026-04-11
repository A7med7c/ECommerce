using ECommerce.DAL.Entities;

namespace EComerce.DAL.Data.Configurations
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).UseIdentityColumn(1, 1);


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


            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(o => o.ShippingAddress)
                   .WithMany(a => a.Orders)
                   .HasForeignKey(o => o.ShippingAddressId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.Property(o => o.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(o => o.ModifiedOn).HasDefaultValueSql("GETDATE()");
            builder.Property(o => o.IsDeleted).HasDefaultValue(false);
        }
    }
}
