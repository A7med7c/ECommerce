using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.DAL.Repositories.Interfaces
{
    /// <summary>
    /// Unit of Work coordinates multiple repository operations under a single
    /// DbContext instance and exposes a single Complete() / CompleteAsync() call
    /// that flushes all staged changes to the database in one transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IAddressRepository Addresses { get; }
        IOrderRepository Orders { get; }
        IFavoriteRepository Favorites { get; }

        /// <summary>Synchronously persists all staged changes. Returns rows affected.</summary>
        int Complete();

        /// <summary>Asynchronously persists all staged changes. Returns rows affected.</summary>
        Task<int> CompleteAsync();

        /// <summary>Begins an explicit database transaction for multi-step operations (e.g. Checkout).</summary>
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
