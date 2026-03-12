using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.Interfaces
{
    public interface IAddressRepository : IGenericRepository<Address>
    {

        IEnumerable<Address> GetByUserId(string userId);
    }
}
