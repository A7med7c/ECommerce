namespace EComerce.DAL.Data.Configurations
{
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Id).UseIdentityColumn(1, 1);
            builder.Property(c => c.Name).HasColumnType("varchar(20)");
            builder.Property(c => c.CreatedOn).HasDefaultValueSql("GETDATE()");
            builder.Property(c => c.ModifiedOn).HasComputedColumnSql("GETDATE()");
        }
    }
}
