using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity>(ApplicationDbContext _dbContext) : IGenericRepository<TEntity> where TEntity : BaseEntity
    {

        private DbSet<TEntity> Table
            => _dbContext.Set<TEntity>();

        // Get All
        public IEnumerable<TEntity> GetAll(bool tracking = false)
        {
            if (tracking)
                return Table.Where(e => !e.IsDeleted).ToList();

            return Table
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .ToList();
        }

        // Get By Id
        public TEntity? GetById(int id)
        {
            return Table
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