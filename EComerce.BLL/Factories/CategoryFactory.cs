using EComerce.DAL.Entities;
using ECommerce.BLL.ViewModels.Category;

namespace ECommerce.BLL.Factories
{
    public static class CategoryFactory
    {
        public static CategoriesVM ToCategoriesVM(this Category category)
        {
            return new CategoriesVM()
            {
                CategoryId = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                CreatedOn = category.CreatedOn.HasValue ? DateOnly.FromDateTime(category.CreatedOn.Value) : default
            };
        }

        public static CategoryDetailsVM ToCategoryDetailsVM(this Category category)
        {
            return new CategoryDetailsVM()
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory != null
                                ? category.ParentCategory.Name
                                : null,
                CreatedBy = category.CreatedBy,
                IsDeleted = category.IsDeleted,
                ModifiedBy = category.ModifiedBy,
                CreatedOn = category.CreatedOn.HasValue ? DateOnly.FromDateTime(category.CreatedOn.Value) : default,
                ModifiedOn = category.ModifiedOn.HasValue ? DateOnly.FromDateTime(category.ModifiedOn.Value) : default,
            };
        }
        public static Category ToEntity(this AddCategoryVM addCategoryVM)
        {
            return new Category()
            {
                Name = addCategoryVM.Name,
                ParentCategoryId = addCategoryVM.ParentCategoryId,
            };
        }
        public static Category ToEntity(this UpdateCategoryVM updateCategoryVM)
        {
            return new Category()
            {
                Id = updateCategoryVM.CategoryId,
                Name = updateCategoryVM.Name,
                ParentCategoryId = updateCategoryVM.ParentCategoryId,
            };
        }
        public static UpdateCategoryVM ToUpdateCategoryVM(this Category category)
        {
            return new UpdateCategoryVM()
            {
                CategoryId = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
            };
        }
    }
}
