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
        // ── Queries ───────────────────────────────────────────────────────

        public IEnumerable<OrdersVM> GetAllOrders()
        {
            var orders = _unitOfWork.Orders.GetAll(
                o => o.ShippingAddress);

            return _mapper.Map<IEnumerable<OrdersVM>>(orders)
                          .OrderByDescending(o => o.OrderDate);
        }

        public OrderDetailsVM? GetOrderById(int id)
        {
            var order = _unitOfWork.Orders.GetById(id,
                o => o.ShippingAddress,
                o => o.OrderItems);

            if (order is null) return null;

            // Eagerly load Product navigation for each item so AutoMapper can
            // resolve ProductName / SKU without lazy-loading.
            foreach (var item in order.OrderItems)
            {
                if (item.Product is null)
                {
                    var product = _unitOfWork.Products.GetById(item.ProductId);
                    if (product is not null) item.Product = product;
                }
            }

            return _mapper.Map<OrderDetailsVM>(order);
        }

        // ── Checkout helpers ──────────────────────────────────────────────

        public CreateOrderVM PrepareCheckoutVM(IReadOnlyList<CartItemVM> cartItems)
        {
            return new CreateOrderVM
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
        }

        // ── Commands ──────────────────────────────────────────────────────

        public int PlaceOrder(CreateOrderVM vm, IReadOnlyList<CartItemVM> cartItems)
        {
            if (!cartItems.Any()) return 0;

            // 1. Persist shipping address
            var address = new Address
            {
                Country = vm.Country,
                City = vm.City,
                Street = vm.Street,
                Zip = vm.Zip,
                IsDefault = false,
                UserId = "guest"          // Replace with real user id when auth is added
            };
            _unitOfWork.Addresses.Add(address);
            _unitOfWork.Complete();          // flush so address gets its Id

            // 2. Build the order
            var order = new Order
            {
                OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                Status = (int)OrderStatus.Pending,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cartItems.Sum(i => i.UnitPrice * i.Quantity),
                ShippingAddressId = address.Id,
                UserId = "guest"  // Replace with real user id when auth is added
            };

            // 3. Build order items
            order.OrderItems = cartItems.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                LineTotal = i.UnitPrice * i.Quantity
            }).ToList();

            _unitOfWork.Orders.Add(order);
            int rows = _unitOfWork.Complete();

            return rows > 0 ? order.Id : 0;
        }

        public bool CancelOrder(int id)
        {
            var order = _unitOfWork.Orders.GetById(id);
            if (order is null) return false;

            order.Status = (int)OrderStatus.Cancelled;
            _unitOfWork.Orders.Update(order);
            return _unitOfWork.Complete() > 0;
        }
    }
}
