using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.DAL.Repositories.Classes
{
    public class AddressRepository(ApplicationDbContext dbContext)
        : GenericRepository<Address>(dbContext), IAddressRepository
    {
        // Uses the protected Table property from GenericRepository to avoid
        // capturing the constructor parameter twice (CS9107).
        public IEnumerable<Address> GetByUserId(string userId)
            => Table.Where(a => a.UserId == userId && !a.IsDeleted).ToList();
    }
}
