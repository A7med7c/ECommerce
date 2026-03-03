using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.DAL.Repositories.Classes
{
    public class ProductRepository(ApplicationDbContext _dbContext) : GenericRepository<Product>(_dbContext), IProductRepository
    {
        public bool SKUExists(string sku, int? id = null)
        {
            return _dbContext.Products.Any(p => p.SKU == sku && (!id.HasValue || p.Id != id));
        }
    }
}
