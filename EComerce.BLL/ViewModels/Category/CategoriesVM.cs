namespace ECommerce.BLL.ViewModels.Category
{
    public class CategoriesVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
        public DateOnly CreatedAt  { get; set; }
    }
}
