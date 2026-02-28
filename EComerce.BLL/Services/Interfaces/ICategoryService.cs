using ECommerce.BLL.ViewModels.Category;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        public AddCategoryVM PrepareCreateVM();
        int AddCategory(AddCategoryVM addCategoryVM);
        bool DeleteCategory(int id);
        IEnumerable<CategoriesVM> GetCategories();
        CategoryDetailsVM? GetCategory(int id);
        int UpdateCategory(UpdateCategoryVM updateCategoryVM);
        public UpdateCategoryVM? GetCategoryForEdit(int id);

    }
}