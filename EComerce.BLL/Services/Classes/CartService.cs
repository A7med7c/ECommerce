using System.Text.Json;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Cart;
using ECommerce.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Services.Classes
{


    public class CartService(IUnitOfWork _unitOfWork, IHttpContextAccessor _httpContextAccessor) : ICartService
    {

        private const string CartSessionKey = "Cart";
        private ISession Session =>
            _httpContextAccessor.HttpContext?.Session
            ?? throw new InvalidOperationException(
                "CartService requires an active HTTP session. " +
                "Make sure UseSession() is called before the request pipeline.");


        public Task<CartVM> GetCartAsync()
        {
            var items = ReadSessionItems();
            return Task.FromResult(new CartVM { Items = items });
        }

        public async Task<(bool Success, string Message)> AddToCartAsync(
            int productId, int quantity = 1)
        {
            if (quantity <= 0)
                return (false, "Quantity must be greater than zero.");


            var product = await _unitOfWork.Products.GetByIdAsync(productId);

            if (product is null)
                return (false, "Product not found.");

            if (!product.IsActive)
                return (false, "This product is currently unavailable.");

            if (product.StockQuantity <= 0)
                return (false, $"'{product.Name}' is out of stock.");


            var items = ReadSessionItems();

            var existing = items.FirstOrDefault(i => i.ProductId == productId);

            if (existing is not null)
            {

                int newQty = existing.Quantity + quantity;

                if (newQty > product.StockQuantity)
                    return (false,
                        $"Only {product.StockQuantity} unit(s) of '{product.Name}' available. " +
                        $"You already have {existing.Quantity} in your cart.");

                existing.Quantity = newQty;
                existing.StockQuantity = product.StockQuantity;
            }
            else
            {
                if (quantity > product.StockQuantity)
                    return (false,
                        $"Only {product.StockQuantity} unit(s) of '{product.Name}' are available.");

                items.Add(new CartItemVM
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    SKU = product.SKU,
                    UnitPrice = product.Price,
                    Quantity = quantity,
                    StockQuantity = product.StockQuantity
                });
            }

            WriteSessionItems(items);
            return (true, $"'{product.Name}' added to cart.");
        }

        public Task<(bool Success, string Message)> UpdateQuantityAsync(
            int productId, int quantity)
        {
            if (quantity <= 0)
            {

                var removed = RemoveItem(productId);
                return Task.FromResult(removed
                    ? (true, "Item removed from cart.")
                    : (false, "Item not found in cart."));
            }

            var items = ReadSessionItems();
            var item = items.FirstOrDefault(i => i.ProductId == productId);

            if (item is null)
                return Task.FromResult((false, "Item not found in cart."));

            if (quantity > item.StockQuantity)
                return Task.FromResult((false,
                    $"Only {item.StockQuantity} unit(s) available."));

            item.Quantity = quantity;
            WriteSessionItems(items);
            return Task.FromResult((true, "Cart updated."));
        }

        public Task<bool> RemoveFromCartAsync(int productId)
            => Task.FromResult(RemoveItem(productId));

        public Task ClearCartAsync()
        {
            Session.Remove(CartSessionKey);
            return Task.CompletedTask;
        }

        public Task<int> GetItemCountAsync()
        {
            var count = ReadSessionItems().Sum(i => i.Quantity);
            return Task.FromResult(count);
        }


        private List<CartItemVM> ReadSessionItems()
        {
            var json = Session.GetString(CartSessionKey);

            return string.IsNullOrEmpty(json)
                ? []
                : JsonSerializer.Deserialize<List<CartItemVM>>(json) ?? [];
        }

        private void WriteSessionItems(List<CartItemVM> items)
            => Session.SetString(CartSessionKey,
                JsonSerializer.Serialize(items));

        private bool RemoveItem(int productId)
        {
            var items = ReadSessionItems();
            var item = items.FirstOrDefault(i => i.ProductId == productId);

            if (item is null) return false;

            items.Remove(item);
            WriteSessionItems(items);
            return true;
        }
    }
}
