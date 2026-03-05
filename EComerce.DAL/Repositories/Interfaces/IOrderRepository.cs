using System.Linq.Expressions;
using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        /// <summary>Returns all non-deleted orders for a specific user, ordered by date desc.</summary>
        IEnumerable<Order> GetOrdersByUserId(
            string userId,
            params Expression<Func<Order, object>>[] includes);

        /// <summary>Async version: returns all non-deleted orders for a specific user, ordered by date desc.</summary>
        Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(
            string userId,
            params Expression<Func<Order, object>>[] includes);

        /// <summary>
        /// Returns a single order with ShippingAddress, OrderItems and each item's
        /// Product loaded in ONE query (no N+1). Returns null when not found.
        /// </summary>
        Task<Order?> GetOrderWithItemsAsync(int id);

        /// <summary>
        /// Same as <see cref="GetOrderWithItemsAsync(int)"/> but also enforces
        /// ownership — returns null when the order does not belong to <paramref name="userId"/>.
        /// </summary>
        Task<Order?> GetOrderWithItemsAsync(int id, string userId);

        /// <summary>
        /// Admin: all orders with User + ShippingAddress loaded to avoid N+1.
        /// Ordered newest first.
        /// </summary>
        Task<IReadOnlyList<Order>> GetAllWithUsersAsync();

        /// <summary>
        /// Admin: single order with User + ShippingAddress + OrderItems + Product.
        /// </summary>
        Task<Order?> GetAdminOrderWithDetailsAsync(int id);
    }
}
