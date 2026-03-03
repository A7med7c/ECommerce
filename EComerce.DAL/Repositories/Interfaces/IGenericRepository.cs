using System.Linq.Expressions;

namespace ECommerce.DAL.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository contract. Does NOT call SaveChanges —
    /// that responsibility belongs to IUnitOfWork.Complete().
    /// </summary>
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);

        IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);

        TEntity? GetById(int id, params Expression<Func<TEntity, object>>[] includes);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);   // soft delete via IsDeleted flag
    }
}
