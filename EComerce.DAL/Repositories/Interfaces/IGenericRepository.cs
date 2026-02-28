namespace ECommerce.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> GetAll(bool tracking = false);
        TEntity? GetById(int id);
        int Add(TEntity entity);
        int Update(TEntity entity);
        int Delete(TEntity entity);
    }
}
