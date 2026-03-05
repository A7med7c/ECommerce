using AutoMapper;
using ECommerce.BLL.Services.Interfaces;
using ECommerce.BLL.ViewModels.Product;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.BLL.Services.Classes
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        // ── Admin queries ──────────────────────────────────────────────────────

        public async Task<ProductDetailsVM?> GetProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id, p => p.Category);
            return product is null ? null : _mapper.Map<ProductDetailsVM>(product);
        }

        public async Task<ProductListVM> GetAdminProductsAsync(
            string? q,
            string sort,
            int page,
            int? categoryId = null,
            int pageSize = 15)
        {
            IQueryable<Product> query = _unitOfWork.Products
                .Query(p => p.Category);     // admin sees ALL products, including inactive

            // ── Filter by category ─────────────────────────────────
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            // ── Search ────────────────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(q))
            {
                string term = q.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    p.SKU.ToLower().Contains(term));
            }

            int totalCount = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            int currentPage = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "name" => query.OrderBy(p => p.Name),
                _ => query.OrderByDescending(p => p.Id)
            };

            var products = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var categories = await _unitOfWork.Categories
                .Query()
                .Select(c => new CategoryFilterVM { Id = c.Id, Name = c.Name })
                .ToListAsync();

            return new ProductListVM
            {
                Products = _mapper.Map<IReadOnlyList<ProductsVM>>(products),
                CurrentPage = currentPage,
                TotalPages = totalPages,
                TotalCount = totalCount,
                PageSize = pageSize,
                CategoryId = categoryId,
                Q = q,
                Sort = sort,
                Categories = categories
            };
        }

        // ── Public catalog ───────────────────────────────────────────────────

        public async Task<ProductListVM> GetCatalogAsync(
            int? categoryId,
            string? q,
            string sort,
            int page,
            int pageSize = 12)
        {
            // Start with IQueryable — nothing goes to the DB yet.
            IQueryable<Product> query = _unitOfWork.Products
                .Query(p => p.Category)     // includes Category for CategoryName
                .Where(p => p.IsActive);    // catalog shows only active products

            // ── Filter by category ─────────────────────────────────
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            // ── Full-text search (name) ────────────────────────────
            if (!string.IsNullOrWhiteSpace(q))
            {
                string term = q.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    p.SKU.ToLower().Contains(term));
            }

            // ── Count BEFORE paging (single COUNT query) ──────────────
            int totalCount = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            int currentPage = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            // ── Sort ────────────────────────────────────────────
            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderByDescending(p => p.Id)  // newest
            };

            // ── Pagination (Skip/Take stays as SQL) ────────────────
            var products = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();             // <─ single DB round-trip

            // ── Build categories dropdown ───────────────────────────
            var categories = await _unitOfWork.Categories
                .Query()
                .Select(c => new CategoryFilterVM { Id = c.Id, Name = c.Name })
                .ToListAsync();

            return new ProductListVM
            {
                Products = _mapper.Map<IReadOnlyList<ProductsVM>>(products),
                CurrentPage = currentPage,
                TotalPages = totalPages,
                TotalCount = totalCount,
                PageSize = pageSize,
                CategoryId = categoryId,
                Q = q,
                Sort = sort,
                Categories = categories
            };
        }

        // ── Commands (async) ──────────────────────────────────────────────────

        public async Task<(bool Success, string Error)> AddProductAsync(ProductCreateUpdateVM vm)
        {
            if (await _unitOfWork.Products.SKUExistsAsync(vm.SKU))
                return (false, $"SKU '{vm.SKU}' is already in use.");

            var product = _mapper.Map<Product>(vm);
            product.CreatedOn = DateTime.UtcNow;

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return (true, string.Empty);
        }

        public async Task<(bool Success, string Error)> UpdateProductAsync(ProductCreateUpdateVM vm)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(vm.Id!.Value);
            if (product is null) return (false, "Product not found.");

            if (await _unitOfWork.Products.SKUExistsAsync(vm.SKU, vm.Id))
                return (false, $"SKU '{vm.SKU}' is already in use.");

            _mapper.Map(vm, product);
            product.ModifiedOn = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.CompleteAsync();
            return (true, string.Empty);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product is null) return false;

            _unitOfWork.Products.Delete(product);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
