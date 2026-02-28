using ECommerce.BLL.Factories;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Category;
using ECommerce.DAL.Repositories.Interfaces;

public class CategoryService(ICategoryRepository repository) : ICategoryService
{
    // Get All
    public IEnumerable<CategoriesVM> GetCategories()
    {
        var categories = repository.GetCategories();
        return categories.Select(c => c.ToCategoriesVM());
    }

    // Details
    public CategoryDetailsVM? GetCategory(int id)
    {
        var category = repository.GetById(id);
        return category is null ? null : category.ToCategoryDetailsVM();
    }

    // ADD
    public int AddCategory(AddCategoryVM vm)
    {
        if (string.IsNullOrWhiteSpace(vm.Name))
            throw new ArgumentException("Category name is required");

        if (vm.ParentCategoryId is not null)
        {
            var parent = repository.GetById(vm.ParentCategoryId.Value);
            if (parent is null)
                throw new Exception("Parent category not found");
        }

        var category = vm.ToEntity();
        return repository.Add(category);
    }

    // GET FOR EDIT
    public UpdateCategoryVM? GetCategoryForEdit(int id)
    {
        var category = repository.GetById(id);
        if (category is null)
            return null;

        return category.ToUpdateCategoryVM();
    }

    // UPDATE
    public int UpdateCategory(UpdateCategoryVM vm)
    {
        var existingCategory = repository.GetById(vm.CategoryId);
        if (existingCategory is null)
            return 0;

        existingCategory.Name = vm.Name;
        existingCategory.ParentCategoryId = vm.ParentCategoryId;

        return repository.Update(existingCategory);
    }

    // DELETE
    public bool DeleteCategory(int id)
    {
        var category = repository.GetById(id);

        if (category is null)
            return false;

        var numberOfRows = repository.Delete(category);

        return numberOfRows > 0;
    }
}