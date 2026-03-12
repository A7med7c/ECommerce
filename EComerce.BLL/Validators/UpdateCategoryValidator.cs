using ECommerce.BLL.ViewModels.Category;
using FluentValidation;

namespace ECommerce.BLL.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryVM>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Invalid category identifier.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");

            RuleFor(x => x.ParentCategoryId)
                .GreaterThan(0)
                .When(x => x.ParentCategoryId.HasValue)
                .WithMessage("Please select a valid parent category.")
                .NotEqual(x => x.CategoryId)
                .When(x => x.ParentCategoryId.HasValue)
                .WithMessage("A category cannot be its own parent.");
        }
    }
}
