using Inventory.Application.Contracts.DTOs;
using Inventory.Application.Contracts.PersistenceInterfaces;
using MediatR;

namespace Inventory.Application.Features.Inventory.Query.GetAllProduct;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, GeneralResponseDto<List<GetProductDto>>>
{
    private readonly IInventoryRepository _inventoryRepository;
    public GetAllProductQueryHandler(IInventoryRepository inventoryRepository) => _inventoryRepository = inventoryRepository;

    public async Task<GeneralResponseDto<List<GetProductDto>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken) => await _inventoryRepository.GetAllProducts(request, cancellationToken).ConfigureAwait(false);
}
