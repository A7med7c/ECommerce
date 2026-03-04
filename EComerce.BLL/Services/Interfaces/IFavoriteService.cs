using ECommerce.BLL.ViewModels.Favorite;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface IFavoriteService
    {
        /// <summary>Returns all favourites for the given user.</summary>
        Task<IEnumerable<FavoriteVM>> GetFavoritesAsync(string userId);

        /// <summary>Returns true if the user has already favourited the product.</summary>
        Task<bool> IsFavoriteAsync(string userId, int productId);

        /// <summary>
        /// Adds the product to the user's favourites.
        /// Returns (false, message) if already favourited or product doesn't exist.
        /// </summary>
        Task<(bool Success, string Message)> AddAsync(string userId, int productId);

        /// <summary>
        /// Removes the product from the user's favourites.
        /// Returns false if the entry was not found.
        /// </summary>
        Task<bool> RemoveAsync(string userId, int productId);
    }
}
