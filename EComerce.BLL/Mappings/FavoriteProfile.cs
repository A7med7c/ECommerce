using AutoMapper;
using ECommerce.BLL.ViewModels.Favorite;
using ECommerce.DAL.Entities;

namespace ECommerce.BLL.Mappings
{
    public class FavoriteProfile : Profile
    {
        public FavoriteProfile()
        {
            CreateMap<Favorite, FavoriteVM>()
                .ForMember(dest => dest.FavoriteId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.Product.StockQuantity))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Product.IsActive))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src =>
                    src.Product.Category != null ? src.Product.Category.Name : string.Empty))
                .ForMember(dest => dest.AddedOn, opt => opt.MapFrom(src => src.CreatedOn));
        }
    }
}
