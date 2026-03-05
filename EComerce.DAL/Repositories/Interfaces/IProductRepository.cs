using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        bool SKUExists(string sku, int? id = null);
        Task<bool> SKUExistsAsync(string sku, int? id = null);
    }
}
