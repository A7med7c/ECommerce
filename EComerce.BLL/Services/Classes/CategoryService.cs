using ECommerce.BLL.Factories;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Category;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.BLL.Services.Classes
{
    public class CategoryService(ICategoryRepository repository) : ICategoryService
    {
        //[GetAll]
        public IEnumerable<CategoriesVM> GetCategories()
        {
            var categories = repository.GetCategories();
            return categories.Select(c => c.ToCategoriesVM());
        }
        //[GetById]
        public CategoryDetailsVM? GetCategory(int id)
        {
            var category = repository.GetById(id);
            return category is null ? null : category.ToCategoryDetailsVM();
        }
        //[Add]
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
        //[Update]
        public int UpdateCategory(UpdateCategoryVM updateCategoryVM)
        {
            return repository.Update(updateCategoryVM.ToEntity());
        }
        //[Delete]
        public bool DeleteCategory(int id)
        {
            var category = repository.GetById(id);

            if (category is null)
                return false;
            var numberOfRows = repository.Delete(category);
            return numberOfRows > 0;
        }
    }
}
