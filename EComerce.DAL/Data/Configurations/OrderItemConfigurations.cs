using ECommerce.DAL.Entities;

namespace EComerce.DAL.Data.Configurations
{
    internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id).UseIdentityColumn(1, 1);


            builder.Property(oi => oi.UnitPrice)
                   .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.LineTotal)
                   .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.Quantity).IsRequired();


            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(oi => oi.Product)
                   .WithMany(p => p.OrderItems)
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.Property(oi => oi.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(oi => oi.ModifiedOn).HasDefaultValueSql("GETDATE()");
            builder.Property(oi => oi.IsDeleted).HasDefaultValue(false);
        }
    }
}
