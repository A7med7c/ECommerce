using System.Linq.Expressions;

namespace ECommerce.DAL.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository contract. Does NOT call SaveChanges —
    /// that responsibility belongs to IUnitOfWork.Complete().
    /// </summary>
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        // ── Sync (kept for backward compat) ───────────────────────────────
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);

        IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);

        TEntity? GetById(int id, params Expression<Func<TEntity, object>>[] includes);

        // ── Async ─────────────────────────────────────────────────────────
        Task<IEnumerable<TEntity>> GetAllAsync(
            params Expression<Func<TEntity, object>>[] includes);

        Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>Asynchronously finds a non-deleted entity by its primary key.</summary>
        Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Returns an <see cref="IQueryable{TEntity}"/> allowing callers to compose
        /// filter / sort / page operations that translate to SQL (no .ToList() in memory).
        /// The query pre-filters on IsDeleted = false and applies any requested includes.
        /// </summary>
        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes);

        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);   // soft delete via IsDeleted flag
    }
}
