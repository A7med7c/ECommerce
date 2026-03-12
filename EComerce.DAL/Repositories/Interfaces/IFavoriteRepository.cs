using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.Interfaces
{


    public interface IFavoriteRepository
    {
        Task<IEnumerable<Favorite>> GetByUserIdAsync(string userId);
        Task<Favorite?> GetAsync(string userId, int productId);
        Task<bool> ExistsAsync(string userId, int productId);
        Task AddAsync(Favorite favorite);
        void Delete(Favorite favorite);
    }
}
