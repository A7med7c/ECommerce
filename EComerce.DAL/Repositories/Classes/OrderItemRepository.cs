using EComerce.DAL.Data.Contexts;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.DAL.Repositories.Classes
{
    public class OrderItemRepository(ApplicationDbContext _dbContext)
        : GenericRepository<OrderItem>(_dbContext), IOrderItemRepository
    {
    }
}
