using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Product;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.BLL.Services.Classes
{
    public class ProductService(IProductRepository _repository) : IProductService
    {

        public IEnumerable<ProductsVM> GetProducts()
        {
            var products = _repository.GetAll(p => p.Category);
            return products.Select(p => new ProductsVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name
            });
        }
        public ProductDetailsVM? GetProduct(int id)
        {
            var product = _repository.GetById(id, p => p.Category);
            if (product is null) return null;
            return new ProductDetailsVM
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            };
        }

        public bool AddProduct(ProductCreateUpdateVM vm)
        {
            if (_repository.SKUExists(vm.SKU))
                return false;
            var product = new Product
            {
                Name = vm.Name,
                SKU = vm.SKU,
                Price = vm.Price,
                StockQuantity = vm.StockQuantity,
                IsActive = vm.IsActive,
                CategoryId = vm.CategoryId,
                CreatedOn = DateTime.UtcNow
            };

            if (_repository.Add(product) > 0)
                return true;
            return false;
        }

        public bool UpdateProduct(ProductCreateUpdateVM vm)
        {
            var product = _repository.GetById(vm.Id!.Value);
            if (product is null) return false;

            if (_repository.SKUExists(vm.SKU, vm.Id))
                return false;

            product.Name = vm.Name;
            product.SKU = vm.SKU;
            product.Price = vm.Price;
            product.StockQuantity = vm.StockQuantity;
            product.IsActive = vm.IsActive;
            product.CategoryId = vm.CategoryId;
            product.ModifiedOn = DateTime.UtcNow;

            return _repository.Update(product) > 0;
        }

        public bool DeleteProduct(int id)
        {
            var product = _repository.GetById(id);
            if (product is null) return false;

            product.IsDeleted = true;
            return _repository.Update(product) > 0 ? true : false;
        }
    }
}
