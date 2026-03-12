using ECommerce.BLL.ViewModels.Cart;

namespace ECommerce.BLL.Services.Interfaces
{


    public interface ICartService
    {

        Task<CartVM> GetCartAsync();


        Task<(bool Success, string Message)> AddToCartAsync(int productId, int quantity = 1);


        Task<(bool Success, string Message)> UpdateQuantityAsync(int productId, int quantity);


        Task<bool> RemoveFromCartAsync(int productId);


        Task ClearCartAsync();


        Task<int> GetItemCountAsync();
    }
}
