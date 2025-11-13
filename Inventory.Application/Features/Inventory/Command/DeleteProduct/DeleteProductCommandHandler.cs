using Inventory.Application.Contracts.DTOs;
using Inventory.Application.Contracts.PersistenceInterfaces;
using MediatR;

namespace Inventory.Application.Features.Inventory.Command.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, GeneralResponseDto<string>>
{
    private readonly IInventoryRepository _inventoryRepository;
    public DeleteProductCommandHandler(IInventoryRepository inventoryRepository) => _inventoryRepository = inventoryRepository;
    public async Task<GeneralResponseDto<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken) => await _inventoryRepository.DeleteProductAsync(request, cancellationToken);
}
