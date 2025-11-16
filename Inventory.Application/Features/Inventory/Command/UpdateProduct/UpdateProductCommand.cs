using Inventory.Application.Contracts.DTOs;
using MediatR;

namespace Inventory.Application.Features.Inventory.Command.UpdateProduct;

public record UpdateProductCommand(Guid Id, string? Name, int? AvailabaleStock, int? ReorderStock) : IRequest<GeneralResponseDto<string>>
{
}
