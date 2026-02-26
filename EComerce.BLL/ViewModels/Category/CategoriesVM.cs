namespace ECommerce.BLL.ViewModels.Category
{
    public class CategoriesVM
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } =string.Empty;
        public int? ParentCategoryId { get; set; }
        public DateOnly CreatedOn { get; set; }
    }
}
