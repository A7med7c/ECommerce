using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.DAL.Repositories.Interfaces
{


    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IAddressRepository Addresses { get; }
        IOrderRepository Orders { get; }
        IFavoriteRepository Favorites { get; }


        int Complete();


        Task<int> CompleteAsync();


        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
