namespace ECommerce.DAL.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        int Add(Category category);
        int Delete(Category category);
        Category? GetById(int id);
        IEnumerable<Category> GetCategories(bool tracking = false);
        int Update(Category category);
    }
}