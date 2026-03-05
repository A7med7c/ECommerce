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

        public async Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(
            string userId,
            params Expression<Func<Order, object>>[] includes)
        {
            IQueryable<Order> query = Table
                .Where(o => o.UserId == userId && !o.IsDeleted);

            foreach (var include in includes)
                query = query.Include(include);

            return await query.OrderByDescending(o => o.OrderDate).ToListAsync();
        }

        public Task<Order?> GetOrderWithItemsAsync(int id)
            => Table
                .Where(o => o.Id == id && !o.IsDeleted)
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync();

        public Task<Order?> GetOrderWithItemsAsync(int id, string userId)
            => Table
                .Where(o => o.Id == id && o.UserId == userId && !o.IsDeleted)
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync();

        public async Task<IReadOnlyList<Order>> GetAllWithUsersAsync()
            => await Table
                .Where(o => !o.IsDeleted)
                .Include(o => o.User)
                .Include(o => o.ShippingAddress)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        public Task<Order?> GetAdminOrderWithDetailsAsync(int id)
            => Table
                .Where(o => o.Id == id && !o.IsDeleted)
                .Include(o => o.User)
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync();
    }
}
