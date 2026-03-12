using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.DAL.Repositories.Classes
{
    
    public class CategoryRepository(ApplicationDbContext _dbContext) : GenericRepository<Category>(_dbContext), ICategoryRepository
    {

    }
}
