using System.Linq.Expressions;

namespace ECommerce.DAL.Repositories.Interfaces
{


    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {

        Task<IEnumerable<TEntity>> GetAllAsync(
            params Expression<Func<TEntity, object>>[] includes);

        Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);


        Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);


        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes);

        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
