using System.Linq.Expressions;
using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {

        IEnumerable<Order> GetOrdersByUserId(
            string userId,
            params Expression<Func<Order, object>>[] includes);


        Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(
            string userId,
            params Expression<Func<Order, object>>[] includes);


        Task<Order?> GetOrderWithItemsAsync(int id);


        Task<Order?> GetOrderWithItemsAsync(int id, string userId);


        Task<IReadOnlyList<Order>> GetAllWithUsersAsync();


        Task<Order?> GetAdminOrderWithDetailsAsync(int id);
    }
}
