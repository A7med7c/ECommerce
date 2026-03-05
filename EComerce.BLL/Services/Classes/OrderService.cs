using AutoMapper;
using ECommerce.BLL.Enums;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Cart;
using ECommerce.BLL.ViewModels.Order;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.BLL.Services.Classes
{
    public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper) : IOrderService
    {
        //  Admin queries 

        public async Task<IEnumerable<AdminOrdersVM>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllWithUsersAsync();
            return _mapper.Map<IEnumerable<AdminOrdersVM>>(orders);
        }

        public async Task<OrderDetailsVM?> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetAdminOrderWithDetailsAsync(id);
            return order is null ? null : _mapper.Map<OrderDetailsVM>(order);
        }

        //  Customer queries 

        public IEnumerable<OrdersVM> GetOrdersByUserId(string userId)
        {
            var orders = _unitOfWork.Orders.GetOrdersByUserId(
                userId,
                o => o.ShippingAddress);
            return _mapper.Map<IEnumerable<OrdersVM>>(orders);
        }

        public async Task<IEnumerable<OrdersVM>> GetOrdersByUserIdAsync(string userId)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByUserIdAsync(
                userId,
                o => o.ShippingAddress);
            return _mapper.Map<IEnumerable<OrdersVM>>(orders);
        }

        public async Task<OrderDetailsVM?> GetOrderByIdAsync(int id, string userId)
        {
            var order = await _unitOfWork.Orders.GetOrderWithItemsAsync(id, userId);
            return order is null ? null : _mapper.Map<OrderDetailsVM>(order);
        }

        //  Checkout helpers 

        public CreateOrderVM PrepareCheckoutVM(IReadOnlyList<CartItemVM> cartItems)
            => new CreateOrderVM
            {
                CartItems = cartItems
                    .Select(i => new CartPreviewItemVM
                    {
                        ProductName = i.ProductName,
                        SKU = i.SKU,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    })
                    .ToList()
            };

        //  Atomic Checkout 

        public async Task<(bool Success, string Error, int OrderId)> PlaceOrderAsync(
            CreateOrderVM vm,
            IReadOnlyList<CartItemVM> cartItems,
            string userId)
        {
            if (!cartItems.Any())
                return (false, "Your cart is empty.", 0);

            await using var txn = await _unitOfWork.BeginTransactionAsync();
            try
            {
                //  1. Load all products in ONE async query (prevent N+1) 
                var productIds = cartItems.Select(i => i.ProductId).ToHashSet();
                var productsList = await _unitOfWork.Products
                    .FindAsync(p => productIds.Contains(p.Id));
                var products = productsList.ToDictionary(p => p.Id);

                //  2. Validate stock for every line 
                foreach (var item in cartItems)
                {
                    if (!products.TryGetValue(item.ProductId, out var product))
                        return (false, $"Product '{item.ProductName}' no longer exists.", 0);

                    if (!product.IsActive)
                        return (false, $"'{product.Name}' is no longer available.", 0);

                    if (product.StockQuantity < item.Quantity)
                        return (false,
                            $"Insufficient stock for '{product.Name}'. " +
                            $"Available: {product.StockQuantity}, requested: {item.Quantity}.", 0);
                }

                //  3. Persist shipping address 
                var address = new Address
                {
                    Country = vm.Country,
                    City = vm.City,
                    Street = vm.Street,
                    Zip = vm.Zip,
                    IsDefault = false,
                    UserId = userId
                };
                _unitOfWork.Addresses.Add(address);

                //  4. Build Order (EF resolves FK via navigation) 
                var order = new Order
                {
                    OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                    Status = (int)OrderStatus.Pending,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = cartItems.Sum(i => i.UnitPrice * i.Quantity),
                    UserId = userId,
                    ShippingAddress = address
                };

                //  5. Build OrderItems 
                order.OrderItems = cartItems.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    LineTotal = i.UnitPrice * i.Quantity
                }).ToList();

                _unitOfWork.Orders.Add(order);

                //  6. Decrement stock 
                foreach (var item in cartItems)
                {
                    var product = products[item.ProductId];
                    product.StockQuantity -= item.Quantity;
                    _unitOfWork.Products.Update(product);
                }

                //  7. Single atomic flush 
                await _unitOfWork.CompleteAsync();

                //  8. Commit 
                await txn.CommitAsync();

                return (true, string.Empty, order.Id);
            }
            catch
            {
                await txn.RollbackAsync();
                return (false, "An error occurred while placing the order. Please try again.", 0);
            }
        }

        //  Commands 

        public async Task<(bool Success, string Error)> UpdateOrderStatusAsync(
            int orderId, int newStatus)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order is null)
                return (false, "Order not found.");

            if (!Enum.IsDefined(typeof(OrderStatus), newStatus))
                return (false, "Invalid status value.");

            var current = (OrderStatus)order.Status;
            var requested = (OrderStatus)newStatus;

            if (requested != OrderStatus.Cancelled && requested <= current)
                return (false,
                    $"Cannot move status from {current} back to {requested}.");

            order.Status = newStatus;
            order.ModifiedOn = DateTime.UtcNow;
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CompleteAsync();

            return (true, string.Empty);
        }

        public async Task<bool> CancelOrderAsync(int id, string userId)
        {
            // Ownership is enforced  only the owner can cancel their order
            var order = await _unitOfWork.Orders.GetOrderWithItemsAsync(id, userId);
            if (order is null) return false;

            order.Status = (int)OrderStatus.Cancelled;
            order.ModifiedOn = DateTime.UtcNow;
            _unitOfWork.Orders.Update(order);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
