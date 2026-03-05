namespace ECommerce.BLL.ViewModels.Product
{
    /// <summary>
    /// Carries a paged + filtered slice of the product catalog.
    /// Passed directly to Catalog/Index — no entity data.
    /// </summary>
    public class ProductListVM
    {
        // ── Paged results ─────────────────────────────────────────────────

        public IReadOnlyList<ProductsVM> Products { get; set; } = [];

        // ── Paging state ──────────────────────────────────────────────────

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        // ── Filter / search state (round-tripped for form persistence) ────

        public int? CategoryId { get; set; }
        public string? Q { get; set; }
        public string Sort { get; set; } = "newest";

        // ── Category list (for filter dropdown) ──────────────────────────

        public IReadOnlyList<CategoryFilterVM> Categories { get; set; } = [];
    }

    /// <summary>Lightweight row used in the catalog filter dropdown.</summary>
    public class CategoryFilterVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
