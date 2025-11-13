using FluentValidation;

namespace Inventory.Application.Features.Inventory.Query.GetAllProduct;

public class GetAllProductValidator : AbstractValidator<GetAllProductQuery>
{
    public GetAllProductValidator()
    {
        RuleFor(x => x.Id).Must(id => id != Guid.Empty).WithMessage("Product ID must be a valid GUID.");
        RuleFor(x => x.ProductName).MaximumLength(250).WithMessage("Product name must not be more than 250 letters");
        RuleFor(x => x.AvailableStockCountFrom).GreaterThan(0).WithMessage("Available stock count from must be greater than 0");
        RuleFor(x => x.ReorderStockCountFrom).GreaterThan(0).WithMessage("Reorder stock count from must be greater than 0");
    }
}
