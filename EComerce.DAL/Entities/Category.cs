using ECommerce.DAL.Entities;

namespace EComerce.DAL.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new HashSet<Category>();
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
