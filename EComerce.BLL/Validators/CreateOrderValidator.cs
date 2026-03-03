using ECommerce.BLL.ViewModels.Order;
using FluentValidation;

namespace ECommerce.BLL.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderVM>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(100);

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100);

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(200);

            RuleFor(x => x.Zip)
                .NotEmpty().WithMessage("ZIP / Postal code is required.")
                .MaximumLength(20);

            RuleFor(x => x.CartItems)
                .Must(items => items != null && items.Count > 0)
                .WithMessage("Your cart is empty. Please add items before placing an order.");
        }
    }
}
