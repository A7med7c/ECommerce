using ECommerce.BLL.ViewModels.Product;

namespace ECommerce.BLL.Services.Interfaces
{
    public interface IProductService
    {
        public IEnumerable<ProductsVM> GetProducts();
        ProductDetailsVM? GetProduct(int id);
        public bool AddProduct(ProductCreateUpdateVM vm);
        public bool UpdateProduct(ProductCreateUpdateVM vm);
        public bool DeleteProduct(int id);
    }
}