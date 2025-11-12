using Inventory.Application.Contracts.DTOs;
using MediatR;

namespace Inventory.Application.Features.Inventory.Command.AddProduct;

public record AddProductCommand(string Name, int AvailableStock, int ReorderStock ) : IRequest<GeneralResponseDto<string>>
{
}
