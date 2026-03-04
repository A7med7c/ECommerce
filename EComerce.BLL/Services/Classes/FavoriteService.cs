using AutoMapper;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Favorite;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.BLL.Services.Classes
{
    public class FavoriteService(IUnitOfWork _unitOfWork, IMapper _mapper) : IFavoriteService
    {
        public async Task<IEnumerable<FavoriteVM>> GetFavoritesAsync(string userId)
        {
            var favorites = await _unitOfWork.Favorites.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<FavoriteVM>>(favorites);
        }

        public Task<bool> IsFavoriteAsync(string userId, int productId)
            => _unitOfWork.Favorites.ExistsAsync(userId, productId);

        public async Task<(bool Success, string Message)> AddAsync(string userId, int productId)
        {
            if (await _unitOfWork.Favorites.ExistsAsync(userId, productId))
                return (false, "Already in your favourites.");

            // Verify the product actually exists and is active
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product is null || !product.IsActive)
                return (false, "Product not found or unavailable.");

            var favorite = new Favorite
            {
                UserId = userId,
                ProductId = productId,
                CreatedOn = DateTime.UtcNow
            };

            await _unitOfWork.Favorites.AddAsync(favorite);
            await _unitOfWork.CompleteAsync();

            return (true, $"'{product.Name}' added to your favourites.");
        }

        public async Task<bool> RemoveAsync(string userId, int productId)
        {
            var favorite = await _unitOfWork.Favorites.GetAsync(userId, productId);
            if (favorite is null) return false;

            _unitOfWork.Favorites.Delete(favorite);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
