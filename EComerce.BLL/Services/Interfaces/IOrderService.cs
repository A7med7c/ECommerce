using ECommerce.BLL.ViewModels.Cart;
using ECommerce.BLL.ViewModels.Order;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface IOrderService
    {


        Task<IEnumerable<AdminOrdersVM>> GetAllOrdersAsync();


        Task<OrderDetailsVM?> GetOrderByIdAsync(int id);


        Task<(bool Success, string Error)> UpdateOrderStatusAsync(int orderId, int newStatus);


        Task<IEnumerable<OrdersVM>> GetOrdersByUserIdAsync(string userId);


        Task<OrderDetailsVM?> GetOrderByIdAsync(int id, string userId);


        CreateOrderVM PrepareCheckoutVM(IReadOnlyList<CartItemVM> cartItems);


        Task<(bool Success, string Error, int OrderId)> PlaceOrderAsync(
            CreateOrderVM vm,
            IReadOnlyList<CartItemVM> cartItems,
            string userId);


        Task<bool> CancelOrderAsync(int id, string userId);
    }
}
