using AutoMapper;
using EComerce.DAL.Entities;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Category;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.BLL.Services.Classes
{
    public class CategoryService(IUnitOfWork _unitOfWork, IMapper _mapper) : ICategoryService
    {
        public IEnumerable<CategoriesVM> GetCategories()
        {
            var categories = _unitOfWork.Categories.GetAll();
            return _mapper.Map<IEnumerable<CategoriesVM>>(categories);
        }

        public CategoryDetailsVM? GetCategory(int id)
        {
            var category = _unitOfWork.Categories.GetById(id, c => c.ParentCategory!);
            if (category is null) return null;
            return _mapper.Map<CategoryDetailsVM>(category);
        }

        public AddCategoryVM PrepareCreateVM()
        {
            return new AddCategoryVM
            {
                ParentCategories = _mapper.Map<List<CategoriesVM>>(_unitOfWork.Categories.GetAll())
            };
        }

        public int AddCategory(AddCategoryVM vm)
        {
            var category = _mapper.Map<Category>(vm);
            _unitOfWork.Categories.Add(category);
            return _unitOfWork.Complete();
        }

        public UpdateCategoryVM? GetCategoryForEdit(int id)
        {
            var category = _unitOfWork.Categories.GetById(id);
            if (category is null) return null;

            var vm = _mapper.Map<UpdateCategoryVM>(category);
            vm.ParentCategories = _mapper.Map<List<CategoriesVM>>(
                _unitOfWork.Categories.GetAll().Where(c => c.Id != id));

            return vm;
        }

        public int UpdateCategory(UpdateCategoryVM vm)
        {
            var existing = _unitOfWork.Categories.GetById(vm.CategoryId);
            if (existing is null) return 0;

            // Map only scalar properties onto the tracked entity
            _mapper.Map(vm, existing);
            _unitOfWork.Categories.Update(existing);
            return _unitOfWork.Complete();
        }

        public bool DeleteCategory(int id)
        {
            var category = _unitOfWork.Categories.GetById(id);
            if (category is null) return false;

            _unitOfWork.Categories.Delete(category);
            return _unitOfWork.Complete() > 0;
        }
    }
}
