using Inventory.Application.Contracts.DTOs;
using MediatR;

namespace Inventory.Application.Features.Inventory.Command.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<GeneralResponseDto<string>>
{
}
