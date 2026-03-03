using System.Linq.Expressions;

namespace ECommerce.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);
        TEntity? GetById(int id, params Expression<Func<TEntity, object>>[] includes);
        int Add(TEntity entity);
        int Update(TEntity entity);
        int Delete(TEntity entity);
    }
}
