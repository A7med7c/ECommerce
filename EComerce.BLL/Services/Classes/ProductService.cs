using AutoMapper;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Product;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.Interfaces;

namespace ECommerce.BLL.Services.Classes
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public IEnumerable<ProductsVM> GetProducts()
        {
            var products = _unitOfWork.Products.GetAll(p => p.Category);
            return _mapper.Map<IEnumerable<ProductsVM>>(products);
        }

        public ProductDetailsVM? GetProduct(int id)
        {
            var product = _unitOfWork.Products.GetById(id, p => p.Category);
            return product is null ? null : _mapper.Map<ProductDetailsVM>(product);
        }

        public bool AddProduct(ProductCreateUpdateVM vm)
        {
            if (_unitOfWork.Products.SKUExists(vm.SKU))
                return false;

            var product = _mapper.Map<Product>(vm);
            product.CreatedOn = DateTime.UtcNow;

            _unitOfWork.Products.Add(product);
            return _unitOfWork.Complete() > 0;
        }

        public bool UpdateProduct(ProductCreateUpdateVM vm)
        {
            var product = _unitOfWork.Products.GetById(vm.Id!.Value);
            if (product is null) return false;

            if (_unitOfWork.Products.SKUExists(vm.SKU, vm.Id))
                return false;

            _mapper.Map(vm, product);
            product.ModifiedOn = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            return _unitOfWork.Complete() > 0;
        }

        public bool DeleteProduct(int id)
        {
            var product = _unitOfWork.Products.GetById(id);
            if (product is null) return false;

            _unitOfWork.Products.Delete(product);
            return _unitOfWork.Complete() > 0;
        }
    }
}
