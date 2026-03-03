using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.DAL.Repositories.Classes
{
    public class ProductRepository(ApplicationDbContext dbContext)
        : GenericRepository<Product>(dbContext), IProductRepository
    {
        public bool SKUExists(string sku, int? id = null)
            => Table.Any(p => p.SKU == sku && (!id.HasValue || p.Id != id));
    }
}
