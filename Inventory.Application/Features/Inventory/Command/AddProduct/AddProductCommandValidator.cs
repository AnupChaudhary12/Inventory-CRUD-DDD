using FluentValidation;

namespace Inventory.Application.Features.Inventory.Command.AddProduct;

public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name must not be empty")
            .MaximumLength(250).WithMessage("Name must not exceed 250 characters");

        RuleFor(x => x.AvailableStock)
            .NotNull().WithMessage("AvailableStocks is required")
            .GreaterThanOrEqualTo(0).WithMessage("AvailableStocks must be a non-negative integer");

        RuleFor(x => x.ReorderStock)
            .NotNull().WithMessage("ReorderStock is required")
            .GreaterThanOrEqualTo(0).WithMessage("ReorderStock must be a non-negative integer");
    }
}
