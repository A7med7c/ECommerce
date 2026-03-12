using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ECommerce.DAL.Repositories.Classes
{


    public class GenericRepository<TEntity>(ApplicationDbContext _dbContext)
        : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbSet<TEntity> Table => _dbContext.Set<TEntity>();


        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Table.Where(e => !e.IsDeleted);
            foreach (var include in includes)
                query = query.Include(include);
            return query.ToList();
        }

        public IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Table.Where(e => !e.IsDeleted).Where(predicate);
            foreach (var include in includes)
                query = query.Include(include);
            return query.ToList();
        }

        public TEntity? GetById(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Table;
            foreach (var include in includes)
                query = query.Include(include);
            return query.FirstOrDefault(e => e.Id == id && !e.IsDeleted);
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync(
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Table.Where(e => !e.IsDeleted);
            foreach (var include in includes)
                query = query.Include(include);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Table.Where(e => !e.IsDeleted).Where(predicate);
            foreach (var include in includes)
                query = query.Include(include);
            return await query.ToListAsync();
        }

        public Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Table;
            foreach (var include in includes)
                query = query.Include(include);
            return query.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Table.Where(e => !e.IsDeleted).AsNoTracking();
            foreach (var include in includes)
                query = query.Include(include);
            return query;
        }

        public void Add(TEntity entity)
            => Table.Add(entity);

        public Task AddAsync(TEntity entity)
            => Table.AddAsync(entity).AsTask();

        public void Update(TEntity entity)
            => Table.Update(entity);

        public void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.ModifiedOn = DateTime.UtcNow;
            Table.Update(entity);
        }
    }
}
