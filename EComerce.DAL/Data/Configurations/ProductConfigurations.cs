using ECommerce.DAL.Entities;

namespace EComerce.DAL.Data.Configurations
{
       internal class ProductConfigurations : IEntityTypeConfiguration<Product>
       {
              public void Configure(EntityTypeBuilder<Product> builder)
              {

                     builder.HasKey(p => p.Id);
                     builder.Property(p => p.Id).UseIdentityColumn(1, 1);


                     builder.Property(p => p.Name)
                            .HasColumnType("varchar(200)")
                            .IsRequired();

                     builder.HasIndex(p => p.Name).IsUnique();

                     builder.Property(p => p.SKU)
                            .HasColumnType("varchar(50)")
                            .IsRequired();

                     builder.HasIndex(p => p.SKU).IsUnique();

                     builder.Property(p => p.Price)
                            .HasColumnType("decimal(18,2)");

                     builder.Property(p => p.Description)
                            .HasColumnType("nvarchar(2000)")
                            .IsRequired(false);

                     builder.Property(p => p.ImageUrl)
                            .HasColumnType("varchar(500)")
                            .IsRequired(false);

                     builder.Property(p => p.StockQuantity)
                            .HasDefaultValue(0);

                     builder.Property(p => p.IsActive)
                            .HasDefaultValue(true);


                     builder.Property(p => p.CreatedOn)
                            .HasColumnName("CreatedAt")
                            .HasDefaultValueSql("GETDATE()");


                     builder.HasOne(p => p.Category)
                            .WithMany(c => c.Products)
                            .HasForeignKey(p => p.CategoryId)
                            .OnDelete(DeleteBehavior.Restrict);


                     builder.Property(p => p.ModifiedOn).HasDefaultValueSql("GETDATE()");
                     builder.Property(p => p.IsDeleted).HasDefaultValue(false);
              }
       }
}
