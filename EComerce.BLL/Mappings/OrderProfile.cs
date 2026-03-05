using AutoMapper;
using ECommerce.BLL.Enums;
using ECommerce.BLL.ViewModels.Order;
using ECommerce.DAL.Entities;

namespace ECommerce.BLL.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // ── Order → OrdersVM ──────────────────────────────────────────
            CreateMap<Order, OrdersVM>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => ((OrderStatus)src.Status).ToString()))
                .ForMember(dest => dest.ShippingCity,
                    opt => opt.MapFrom(src =>
                        src.ShippingAddress != null ? src.ShippingAddress.City : string.Empty));

            // ── Order → AdminOrdersVM (includes user info for admin list) ──
            CreateMap<Order, AdminOrdersVM>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => ((OrderStatus)src.Status).ToString()))
                .ForMember(dest => dest.ShippingCity,
                    opt => opt.MapFrom(src =>
                        src.ShippingAddress != null ? src.ShippingAddress.City : string.Empty))
                .ForMember(dest => dest.UserEmail,
                    opt => opt.MapFrom(src =>
                        src.User != null ? src.User.Email : string.Empty))
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src =>
                        src.User != null ? src.User.FullName : string.Empty));

            // ── Order → OrderDetailsVM ────────────────────────────────────
            CreateMap<Order, OrderDetailsVM>()
                .ForMember(dest => dest.StatusValue,
                    opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => ((OrderStatus)src.Status).ToString()))
                .ForMember(dest => dest.ShippingCountry,
                    opt => opt.MapFrom(src =>
                        src.ShippingAddress != null ? src.ShippingAddress.Country : string.Empty))
                .ForMember(dest => dest.ShippingCity,
                    opt => opt.MapFrom(src =>
                        src.ShippingAddress != null ? src.ShippingAddress.City : string.Empty))
                .ForMember(dest => dest.ShippingStreet,
                    opt => opt.MapFrom(src =>
                        src.ShippingAddress != null ? src.ShippingAddress.Street : string.Empty))
                .ForMember(dest => dest.ShippingZip,
                    opt => opt.MapFrom(src =>
                        src.ShippingAddress != null ? src.ShippingAddress.Zip : string.Empty))
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.OrderItems));

            // ── OrderItem → OrderItemVM ───────────────────────────────────
            CreateMap<OrderItem, OrderItemVM>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src =>
                        src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(dest => dest.SKU,
                    opt => opt.MapFrom(src =>
                        src.Product != null ? src.Product.SKU : string.Empty));
        }
    }
}
