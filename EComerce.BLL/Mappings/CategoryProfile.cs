using AutoMapper;
using EComerce.DAL.Entities;
using ECommerce.BLL.ViewModels.Category;

namespace ECommerce.BLL.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {


            CreateMap<Category, CategoriesVM>()
                .ForMember(dest => dest.CreatedOn,
                    opt => opt.MapFrom(src =>
                        src.CreatedOn.HasValue
                            ? DateOnly.FromDateTime(src.CreatedOn.Value)
                            : default));

            CreateMap<Category, CategoryDetailsVM>()
                .ForMember(dest => dest.ParentCategoryName,
                    opt => opt.MapFrom(src =>
                        src.ParentCategory != null ? src.ParentCategory.Name : null))
                .ForMember(dest => dest.CreatedOn,
                    opt => opt.MapFrom(src =>
                        src.CreatedOn.HasValue
                            ? DateOnly.FromDateTime(src.CreatedOn.Value)
                            : default))
                .ForMember(dest => dest.ModifiedOn,
                    opt => opt.MapFrom(src =>
                        src.ModifiedOn.HasValue
                            ? DateOnly.FromDateTime(src.ModifiedOn.Value)
                            : default));

            CreateMap<Category, UpdateCategoryVM>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ParentCategories, opt => opt.Ignore());


            CreateMap<AddCategoryVM, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedOn, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());


            CreateMap<UpdateCategoryVM, Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedOn, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());
        }
    }
}
