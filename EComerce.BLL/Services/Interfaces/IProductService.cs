using ECommerce.BLL.ViewModels.Product;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface IProductService
    {


        Task<ProductListVM> GetAdminProductsAsync(
            string? q,
            string sort,
            int page,
            int? categoryId = null,
            int pageSize = 15);


        Task<ProductDetailsVM?> GetProductAsync(int id);


        Task<ProductListVM> GetCatalogAsync(
            int? categoryId,
            string? q,
            string sort,
            int page,
            int pageSize = 12);


        Task<(bool Success, string Error)> AddProductAsync(ProductCreateUpdateVM vm);
        Task<(bool Success, string Error)> UpdateProductAsync(ProductCreateUpdateVM vm);
        Task<bool> DeleteProductAsync(int id);
    }
}
