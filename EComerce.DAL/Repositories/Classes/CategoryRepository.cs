using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.DAL.Repositories.Classes
{
    /* Steps Of Make DI
        [1].Inject needed object
        [2]. Life Time Of These Object[Memory] in IOC(in program.cs) 
   */
    public class CategoryRepository(ApplicationDbContext _dbContext) : ICategoryRepository
    {

        //[GetAllCategories]
        public IEnumerable<Category> GetCategories(bool tracking = false) => _dbContext.Categories.ToList();
        //[GatDepartmentByID]
        public Category? GetById(int id)
        {
            return _dbContext.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefault(c => c.Id == id);
        }// find -> must take id and if the value exists in the cash will get value from it.

        //[Add]
        public int Add(Category category)
        {
            _dbContext.Categories.Add(category);
            return _dbContext.SaveChanges();
        }
        //[Update]
        public int Update(Category category)
        {
            _dbContext.Categories.Update(category);
            return _dbContext.SaveChanges();
        }
        //[Delete]
        public int Delete(Category category)
        {
            _dbContext.Categories.Remove(category);
            return _dbContext.SaveChanges();
        }
    }
}
