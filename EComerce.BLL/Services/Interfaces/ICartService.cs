using ECommerce.BLL.ViewModels.Cart;

namespace ECommerce.BLL.Services.Interfaces
{
    /// <summary>
    /// Session-backed shopping cart service.
    /// All operations are async so they compose cleanly with async controllers.
    /// </summary>
    public interface ICartService
    {
        /// <summary>Returns the current cart for the active session.</summary>
        Task<CartVM> GetCartAsync();

        /// <summary>
        /// Adds <paramref name="quantity"/> units of a product to the cart.
        /// Returns (false, message) if the product does not exist, is inactive,
        /// or if the requested quantity exceeds available stock.
        /// Increments quantity if the product is already in the cart.
        /// </summary>
        Task<(bool Success, string Message)> AddToCartAsync(int productId, int quantity = 1);

        /// <summary>
        /// Sets the quantity of a cart line to <paramref name="quantity"/>.
        /// Returns (false, message) if stock is insufficient.
        /// If <paramref name="quantity"/> is 0 or less, removes the line.
        /// </summary>
        Task<(bool Success, string Message)> UpdateQuantityAsync(int productId, int quantity);

        /// <summary>
        /// Removes an item from the cart.
        /// Returns false if the item was not present.
        /// </summary>
        Task<bool> RemoveFromCartAsync(int productId);

        /// <summary>Removes all items from the session cart.</summary>
        Task ClearCartAsync();

        /// <summary>Returns the total number of individual units in the cart.</summary>
        Task<int> GetItemCountAsync();
    }
}
