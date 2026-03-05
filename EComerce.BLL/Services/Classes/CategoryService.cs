using AutoMapper;
using EComerce.DAL.Entities;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Category;
using ECommerce.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.BLL.Services.Classes
{
    public class CategoryService(IUnitOfWork _unitOfWork, IMapper _mapper) : ICategoryService
    {
        // ── Queries ───────────────────────────────────────────────────────

        public IEnumerable<CategoriesVM> GetCategories()
        {
            var categories = _unitOfWork.Categories.GetAll();
            return _mapper.Map<IEnumerable<CategoriesVM>>(categories);
        }

        public async Task<IEnumerable<CategoriesVM>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoriesVM>>(categories);
        }

        public CategoryDetailsVM? GetCategory(int id)
        {
            var category = _unitOfWork.Categories.GetById(id, c => c.ParentCategory!);
            if (category is null) return null;
            return _mapper.Map<CategoryDetailsVM>(category);
        }

        public async Task<CategoryDetailsVM?> GetCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id, c => c.ParentCategory!);
            if (category is null) return null;
            return _mapper.Map<CategoryDetailsVM>(category);
        }

        // ── Prepare VMs ───────────────────────────────────────────────────

        public AddCategoryVM PrepareCreateVM()
        {
            return new AddCategoryVM
            {
                ParentCategories = _mapper.Map<List<CategoriesVM>>(_unitOfWork.Categories.GetAll())
            };
        }

        public async Task<AddCategoryVM> PrepareCreateVMAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return new AddCategoryVM
            {
                ParentCategories = _mapper.Map<List<CategoriesVM>>(categories)
            };
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

        public async Task<UpdateCategoryVM?> GetCategoryForEditAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category is null) return null;

            var vm = _mapper.Map<UpdateCategoryVM>(category);
            var others = await _unitOfWork.Categories
                .Query()
                .Where(c => c.Id != id)
                .ToListAsync();
            vm.ParentCategories = _mapper.Map<List<CategoriesVM>>(others);

            return vm;
        }

        // ── Commands (sync) ───────────────────────────────────────────────

        public int AddCategory(AddCategoryVM vm)
        {
            var category = _mapper.Map<Category>(vm);
            _unitOfWork.Categories.Add(category);
            return _unitOfWork.Complete();
        }

        public int UpdateCategory(UpdateCategoryVM vm)
        {
            var existing = _unitOfWork.Categories.GetById(vm.CategoryId);
            if (existing is null) return 0;

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

        // ── Commands (async) ──────────────────────────────────────────────

        public async Task<int> AddCategoryAsync(AddCategoryVM vm)
        {
            var category = _mapper.Map<Category>(vm);
            await _unitOfWork.Categories.AddAsync(category);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateCategoryAsync(UpdateCategoryVM vm)
        {
            var existing = await _unitOfWork.Categories.GetByIdAsync(vm.CategoryId);
            if (existing is null) return 0;

            _mapper.Map(vm, existing);
            _unitOfWork.Categories.Update(existing);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category is null) return false;

            _unitOfWork.Categories.Delete(category);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
