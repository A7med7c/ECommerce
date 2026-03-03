using System.Linq.Expressions;
using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.DAL.Repositories.Classes
{
    public class OrderRepository(ApplicationDbContext dbContext)
        : GenericRepository<Order>(dbContext), IOrderRepository
    {
        public IEnumerable<Order> GetOrdersByUserId(
            string userId,
            params Expression<Func<Order, object>>[] includes)
        {
            IQueryable<Order> query = Table
                .Where(o => o.UserId == userId && !o.IsDeleted);

            foreach (var include in includes)
                query = query.Include(include);

            return query.OrderByDescending(o => o.OrderDate).ToList();
        }
    }
}
