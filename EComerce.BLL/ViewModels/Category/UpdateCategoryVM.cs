namespace ECommerce.BLL.ViewModels.Category
{
    public class UpdateCategoryVM
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
        public DateOnly CreatedOn { get; set; }
    }
}