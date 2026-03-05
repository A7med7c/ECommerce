using ECommerce.DAL.Entities;

namespace ECommerce.DAL.Repositories.Interfaces
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        /// <summary>Returns all non-deleted addresses that belong to a given user.</summary>
        IEnumerable<Address> GetByUserId(string userId);
    }
}
