using Inventory.Application.Contracts.DTOs;
using Inventory.Application.Contracts.PersistenceInterfaces;
using MediatR;

namespace Inventory.Application.Features.Inventory.Command.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, GeneralResponseDto<string>>
{
    private readonly IInventoryRepository _inventoryRepository;
    public UpdateProductCommandHandler(IInventoryRepository inventoryRepository) => _inventoryRepository = inventoryRepository;

    public async Task<GeneralResponseDto<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken) => await _inventoryRepository.UpdateProductAsync(request, cancellationToken);
}
