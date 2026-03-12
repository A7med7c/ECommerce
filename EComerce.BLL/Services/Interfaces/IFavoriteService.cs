using ECommerce.BLL.ViewModels.Favorite;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface IFavoriteService
    {

        Task<IEnumerable<FavoriteVM>> GetFavoritesAsync(string userId);


        Task<bool> IsFavoriteAsync(string userId, int productId);


        Task<(bool Success, string Message)> AddAsync(string userId, int productId);


        Task<bool> RemoveAsync(string userId, int productId);
    }
}
