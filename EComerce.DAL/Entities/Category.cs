namespace EComerce.DAL.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int? ParentCategoryId { get; set; }
    }
}
