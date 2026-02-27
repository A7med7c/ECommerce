using ECommerce.BLL.ViewModels.Category;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        int AddCategory(AddCategoryVM addCategoryVM);
        bool DeleteCategory(int id);
        IEnumerable<CategoriesVM> GetCategories();
        CategoryDetailsVM? GetCategory(int id);
        int UpdateCategory(UpdateCategoryVM updateCategoryVM);
    }
}