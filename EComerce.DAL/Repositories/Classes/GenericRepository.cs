using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ECommerce.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity>(ApplicationDbContext _dbContext) : IGenericRepository<TEntity> where TEntity : BaseEntity
    {

        private DbSet<TEntity> Table
            => _dbContext.Set<TEntity>();

        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Table.Where(e => !e.IsDeleted);

            foreach (var include in includes)
                query = query.Include(include);

            return query.ToList();
        }

        // Get By Id
        public TEntity? GetById(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Table;

            foreach (var include in includes)
                query = query.Include(include);

            return query
                .FirstOrDefault(e => e.Id == id && !e.IsDeleted);
        }

        // Add
        public int Add(TEntity entity)
        {
            Table.Add(entity);
            return _dbContext.SaveChanges();
        }

        // Update
        public int Update(TEntity entity)
        {
            Table.Update(entity);
            return _dbContext.SaveChanges();
        }

        // Soft Delete
        public int Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.ModifiedOn = DateTime.UtcNow;

            Table.Update(entity);
            return _dbContext.SaveChanges();
        }
    }
}