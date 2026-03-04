using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.DAL.Repositories.Classes
{
    /// <summary>
    /// Concrete Unit of Work. Shares a single ApplicationDbContext across all
    /// child repositories and exposes Complete() as the single save point.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(_context);
            Products = new ProductRepository(_context);
            Addresses = new AddressRepository(_context);
            Orders = new OrderRepository(_context);
            Favorites = new FavoriteRepository(_context);
        }

        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public IAddressRepository Addresses { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IFavoriteRepository Favorites { get; private set; }

        public int Complete() => _context.SaveChanges();
        public Task<int> CompleteAsync() => _context.SaveChangesAsync();

        public IDbContextTransaction BeginTransaction() => _context.Database.BeginTransaction();
        public Task<IDbContextTransaction> BeginTransactionAsync() => _context.Database.BeginTransactionAsync();

        public void Dispose() => _context.Dispose();
    }
}
