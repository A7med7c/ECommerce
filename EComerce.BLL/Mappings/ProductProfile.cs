using AutoMapper;
using ECommerce.BLL.ViewModels.Product;
using ECommerce.DAL.Entities;

namespace ECommerce.BLL.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {


            CreateMap<Product, ProductsVM>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, ProductDetailsVM>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name));


            CreateMap<Product, ProductCreateUpdateVM>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore());


            CreateMap<ProductCreateUpdateVM, Product>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id ?? 0))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
                .ForMember(dest => dest.Favorites, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedOn, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());
        }
    }
}
