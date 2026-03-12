using ECommerce.BLL.ViewModels.Product;
using FluentValidation;

namespace ECommerce.BLL.Validators
{
    public class ProductCreateUpdateValidator : AbstractValidator<ProductCreateUpdateVM>
    {
        public ProductCreateUpdateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU is required.")
                .MaximumLength(50).WithMessage("SKU cannot exceed 50 characters.")
                .Matches(@"^[A-Za-z0-9\-_]+$").WithMessage("SKU may only contain letters, digits, hyphens, and underscores.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Please select a valid category.");
        }
    }
}
