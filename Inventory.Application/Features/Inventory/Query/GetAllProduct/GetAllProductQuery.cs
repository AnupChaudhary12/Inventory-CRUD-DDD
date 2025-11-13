using Inventory.Application.Contracts.DTOs;
using MediatR;

namespace Inventory.Application.Features.Inventory.Query.GetAllProduct;

public record GetAllProductQuery(Guid? Id, string? ProductName, int? AvailableStockCountFrom, int? AvailableStockCountTo, int? ReorderStockCountFrom, int? ReorderStockCountTo, int? PageNo, int? PageSize ) : IRequest<GeneralResponseDto<List<GetProductDto>>>
{
}
