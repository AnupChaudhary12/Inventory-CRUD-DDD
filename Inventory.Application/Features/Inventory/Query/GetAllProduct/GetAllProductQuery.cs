using Inventory.Application.Contracts.DTOs;
using MediatR;

namespace Inventory.Application.Features.Inventory.Query.GetAllProduct;

public record GetAllProductQuery : IRequest<GeneralResponseDto<List<GetProductDto>>>
{
}
