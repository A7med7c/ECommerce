using ECommerce.BLL.ViewModels.Category;
using FluentValidation;

namespace ECommerce.BLL.Validators
{
    public class AddCategoryValidator : AbstractValidator<AddCategoryVM>
    {
        public AddCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");

            RuleFor(x => x.ParentCategoryId)
                .GreaterThan(0)
                .When(x => x.ParentCategoryId.HasValue)
                .WithMessage("Please select a valid parent category.");
        }
    }
}
