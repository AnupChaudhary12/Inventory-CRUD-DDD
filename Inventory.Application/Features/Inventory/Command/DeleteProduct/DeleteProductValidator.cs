using FluentValidation;

namespace Inventory.Application.Features.Inventory.Command.DeleteProduct;

public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator() => RuleFor(x => x.Id)
                   .NotEmpty().WithMessage("Product ID must not be empty.")
                   .Must(id => id != Guid.Empty).WithMessage("Product ID must be a valid GUID.");
}
