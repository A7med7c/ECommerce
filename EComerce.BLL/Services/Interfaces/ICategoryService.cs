using ECommerce.BLL.ViewModels.Category;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface ICategoryService
    {

        Task<IEnumerable<CategoriesVM>> GetCategoriesAsync();


        Task<CategoryDetailsVM?> GetCategoryAsync(int id);
        Task<AddCategoryVM> PrepareCreateVMAsync();
        Task<int> AddCategoryAsync(AddCategoryVM vm);
        Task<UpdateCategoryVM?> GetCategoryForEditAsync(int id);
        Task<int> UpdateCategoryAsync(UpdateCategoryVM vm);
        Task<bool> DeleteCategoryAsync(int id);
    }
}