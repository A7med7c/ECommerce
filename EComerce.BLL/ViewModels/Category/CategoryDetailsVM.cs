namespace ECommerce.BLL.ViewModels.Category
{
    public class CategoryDetailsVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int? ParentCategoryId { get; set; }

        public string? ParentCategoryName { get; set; }

        public int CreatedBy { get; set; }

        public DateOnly CreatedOn { get; set; }

        public int ModifiedBy { get; set; }

        public DateOnly ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}