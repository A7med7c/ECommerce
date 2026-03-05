using ECommerce.BLL.ViewModels.Cart;
using ECommerce.BLL.ViewModels.Order;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        // ── Admin queries ─────────────────────────────────────────────────

        /// <summary>Returns all non-deleted orders for the admin list (includes user info).</summary>
        Task<IEnumerable<AdminOrdersVM>> GetAllOrdersAsync();

        /// <summary>
        /// Returns a single order (with items + address + user) by id — admin use only.
        /// Does NOT enforce ownership.
        /// </summary>
        Task<OrderDetailsVM?> GetOrderByIdAsync(int id);

        // ── Admin commands ──────────────────────────────────────────────

        /// <summary>
        /// Updates the order status. Only allows forward progression of status.
        /// Returns (Success, ErrorMessage).
        /// </summary>
        Task<(bool Success, string Error)> UpdateOrderStatusAsync(int orderId, int newStatus);

        // ── Customer queries ──────────────────────────────────────────────

        /// <summary>Returns all orders that belong to <paramref name="userId"/>.</summary>
        IEnumerable<OrdersVM> GetOrdersByUserId(string userId);

        /// <summary>Async version: returns all orders that belong to <paramref name="userId"/>.</summary>
        Task<IEnumerable<OrdersVM>> GetOrdersByUserIdAsync(string userId);

        /// <summary>
        /// Returns an order's detail only when it belongs to <paramref name="userId"/>.
        /// Returns <c>null</c> when not found or the order belongs to a different user.
        /// </summary>
        Task<OrderDetailsVM?> GetOrderByIdAsync(int id, string userId);

        // ── Checkout ──────────────────────────────────────────────────────

        /// <summary>Populates the checkout VM with a read-only preview of the current cart.</summary>
        CreateOrderVM PrepareCheckoutVM(IReadOnlyList<CartItemVM> cartItems);

        /// <summary>
        /// Atomically validates stock, creates Address + Order + OrderItems,
        /// decrements stock, and saves everything in ONE EF Core transaction.
        /// Returns (Success, ErrorMessage, NewOrderId).
        /// </summary>
        Task<(bool Success, string Error, int OrderId)> PlaceOrderAsync(
            CreateOrderVM vm,
            IReadOnlyList<CartItemVM> cartItems,
            string userId);

        // ── Commands ──────────────────────────────────────────────────────

        /// <summary>Cancels an order that belongs to <paramref name="userId"/>. Returns false when not found or not owned.</summary>
        Task<bool> CancelOrderAsync(int id, string userId);
    }
}
