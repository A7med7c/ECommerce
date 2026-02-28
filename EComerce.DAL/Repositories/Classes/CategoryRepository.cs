using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.DAL.Repositories.Classes
{
    /* Steps Of Make DI
        [1].Inject needed object
        [2]. Life Time Of These Object[Memory] in IOC(in program.cs) 
   */
    public class CategoryRepository(ApplicationDbContext _dbContext) : GenericRepository<Category>(_dbContext), ICategoryRepository
    {

    }
}
