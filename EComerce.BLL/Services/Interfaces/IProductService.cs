using ECommerce.BLL.ViewModels.Product;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface IProductService
    {
        //  Admin queries 

        /// <summary>Paged, searchable, sortable list for admin grid.</summary>
        Task<ProductListVM> GetAdminProductsAsync(
            string? q,
            string sort,
            int page,
            int? categoryId = null,
            int pageSize = 15);

        /// <summary>Single product detail for admin/customer detail page.</summary>
        Task<ProductDetailsVM?> GetProductAsync(int id);

        //  Public catalog 

        /// <summary>
        /// Returns a paged, filtered, searched and sorted slice of active products
        /// for the public catalog. Builds an IQueryable pipeline  no ToList() before filtering.
        /// </summary>
        Task<ProductListVM> GetCatalogAsync(
            int? categoryId,
            string? q,
            string sort,
            int page,
            int pageSize = 12);

        //  Commands 

        Task<(bool Success, string Error)> AddProductAsync(ProductCreateUpdateVM vm);
        Task<(bool Success, string Error)> UpdateProductAsync(ProductCreateUpdateVM vm);
        Task<bool> DeleteProductAsync(int id);
    }
}
