namespace ECommerce.BLL.ViewModels.Product
{
    public class ProductsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
