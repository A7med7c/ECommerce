namespace ECommerce.BLL.ViewModels.Category
{
    public class AddCategoryVM
    {
        public string Name { get; set; } = string.Empty;

        public int? ParentCategoryId { get; set; }

        public List<CategoriesVM> ParentCategories { get; set; } = new();
    }
}