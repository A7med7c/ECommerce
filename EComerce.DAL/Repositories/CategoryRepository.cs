using EComerce.DAL.Data.Contexts;

namespace EComerce.DAL.Repositories
{
    internal class CategoryRepository(ApplicationDbContext _dbContext)
    {

        /* Steps Of Make DI
            [1].Inject needed object
            [2]. Life Time Of These Object[Memory] in IOC(in program.cs) 
       */

        public Category? GetById(int id)
        {
            return _dbContext.Categories.Find(id); //must take id and if the value exists in the cash will get value from it.
        }
    }
}
