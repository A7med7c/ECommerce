using ECommerce.BLL.ViewModels.Cart;
using ECommerce.BLL.ViewModels.Order;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        /// <summary>Returns all non-deleted orders (admin list).</summary>
        IEnumerable<OrdersVM> GetAllOrders();

        /// <summary>Returns a single order with its items and shipping address, or null.</summary>
        OrderDetailsVM? GetOrderById(int id);

        /// <summary>
        /// Populates the checkout VM with a read-only preview of the current cart items.
        /// </summary>
        CreateOrderVM PrepareCheckoutVM(IReadOnlyList<CartItemVM> cartItems);

        /// <summary>
        /// Persists a new Order + Address from the checkout VM and the live cart items.
        /// Returns the new Order Id on success, or 0 on failure.
        /// </summary>
        int PlaceOrder(CreateOrderVM vm, IReadOnlyList<CartItemVM> cartItems);

        /// <summary>Soft-deletes (cancels) an order. Returns false when not found.</summary>
        bool CancelOrder(int id);
    }
}
