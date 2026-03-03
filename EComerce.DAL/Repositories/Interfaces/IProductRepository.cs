using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public bool SKUExists(string sku, int? id = null);
    }
}
