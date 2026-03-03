using System.Linq.Expressions;
using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        /// <summary>Returns all non-deleted orders for a specific user.</summary>
        IEnumerable<Order> GetOrdersByUserId(
            string userId,
            params Expression<Func<Order, object>>[] includes);
    }
}
