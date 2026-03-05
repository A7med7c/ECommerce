using ECommerce.BLL.ViewModels.Category;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        // ── Sync (kept for internal/simple use) ──────────────────────────
        IEnumerable<CategoriesVM> GetCategories();
        Task<IEnumerable<CategoriesVM>> GetCategoriesAsync();

        // ── Async CRUD ────────────────────────────────────────────────────
        Task<CategoryDetailsVM?> GetCategoryAsync(int id);
        Task<AddCategoryVM> PrepareCreateVMAsync();
        Task<int> AddCategoryAsync(AddCategoryVM vm);
        Task<UpdateCategoryVM?> GetCategoryForEditAsync(int id);
        Task<int> UpdateCategoryAsync(UpdateCategoryVM vm);
        Task<bool> DeleteCategoryAsync(int id);

        // ── Sync (kept for backward compat — avoid in new code) ──────────
        CategoryDetailsVM? GetCategory(int id);
        AddCategoryVM PrepareCreateVM();
        int AddCategory(AddCategoryVM vm);
        UpdateCategoryVM? GetCategoryForEdit(int id);
        int UpdateCategory(UpdateCategoryVM vm);
        bool DeleteCategory(int id);
    }
}