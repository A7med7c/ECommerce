using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL.Repositories.Classes
{
    public class FavoriteRepository(ApplicationDbContext dbContext) : IFavoriteRepository
    {
        private DbSet<Favorite> Table => dbContext.Set<Favorite>();

        public async Task<IEnumerable<Favorite>> GetByUserIdAsync(string userId)
            => await Table
                .Where(f => f.UserId == userId)
                .Include(f => f.Product)
                    .ThenInclude(p => p.Category)
                .OrderByDescending(f => f.CreatedOn)
                .ToListAsync();

        public async Task<Favorite?> GetAsync(string userId, int productId)
            => await Table.FirstOrDefaultAsync(
                f => f.UserId == userId && f.ProductId == productId);

        public async Task<bool> ExistsAsync(string userId, int productId)
            => await Table.AnyAsync(
                f => f.UserId == userId && f.ProductId == productId);

        public async Task AddAsync(Favorite favorite)
            => await Table.AddAsync(favorite);

        public void Delete(Favorite favorite)
            => Table.Remove(favorite);
    }
}
