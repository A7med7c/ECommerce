using ECommerce.BLL.Factories;
using ECommerce.BLL.ViewModels.Category;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.BLL.Services.Interfaces
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
        public int AddCategory(AddCategoryVM addCategoryVM)
        {
            return repository.Add(addCategoryVM.ToEntity());
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
