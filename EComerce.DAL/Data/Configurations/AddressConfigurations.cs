using ECommerce.DAL.Entities;
using ECommerce.DAL.Entities.IdentityModule;

namespace EComerce.DAL.Data.Configurations
{
    internal class AddressConfigurations : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            // PK
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).UseIdentityColumn(1, 1);

            // Properties
            builder.Property(a => a.Country)
                   .HasColumnType("varchar(100)")
                   .IsRequired();

            builder.Property(a => a.City)
                   .HasColumnType("varchar(100)")
                   .IsRequired();

            builder.Property(a => a.Street)
                   .HasColumnType("varchar(200)")
                   .IsRequired();

            builder.Property(a => a.Zip)
                   .HasColumnType("varchar(20)")
                   .IsRequired();

            builder.Property(a => a.IsDefault)
                   .HasDefaultValue(false);

            builder.Property(a => a.UserId).IsRequired();

            // Relationship: Customer 1→∞ Address
            builder.HasOne(a => a.ApplicationUser)
                   .WithMany(u => u.Addresses)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Audit columns
            builder.Property(a => a.CreatedOn).HasDefaultValueSql("GETDATE()");
            builder.Property(a => a.ModifiedOn).HasDefaultValueSql("GETDATE()");
            builder.Property(a => a.IsDeleted).HasDefaultValue(false);
        }
    }
}
