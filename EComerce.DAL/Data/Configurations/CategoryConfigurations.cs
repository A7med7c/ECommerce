namespace EComerce.DAL.Data.Configurations
{
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // PK
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).UseIdentityColumn(1, 1);

            // Properties
            builder.Property(c => c.Name)
                   .HasColumnType("varchar(100)")
                   .IsRequired();

            builder.HasIndex(c => c.Name).IsUnique();

            // Self-referencing: Category 1→∞ SubCategories
            builder.HasOne(c => c.ParentCategory)
                   .WithMany(c => c.SubCategories)
                   .HasForeignKey(c => c.ParentCategoryId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            // Audit columns
            builder.Property(c => c.CreatedOn).HasDefaultValueSql("GETDATE()");
            builder.Property(c => c.ModifiedOn).HasDefaultValueSql("GETDATE()");
            builder.Property(c => c.IsDeleted).HasDefaultValue(false);
        }
    }
}
