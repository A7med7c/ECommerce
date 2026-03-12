namespace ECommerce.BLL.ViewModels.Product
{


    public class ProductListVM
    {


        public IReadOnlyList<ProductsVM> Products { get; set; } = [];


        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;


        public int? CategoryId { get; set; }
        public string? Q { get; set; }
        public string Sort { get; set; } = "newest";


        public IReadOnlyList<CategoryFilterVM> Categories { get; set; } = [];
    }


    public class CategoryFilterVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
