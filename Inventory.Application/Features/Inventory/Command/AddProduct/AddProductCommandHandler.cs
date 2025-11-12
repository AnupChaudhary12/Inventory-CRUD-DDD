using Inventory.Application.Contracts.DTOs;
using Inventory.Application.Contracts.PersistenceInterfaces;
using MediatR;

namespace Inventory.Application.Features.Inventory.Command.AddProduct;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, GeneralResponseDto<string>>
{
    private readonly IInventoryRepository _inventoryRepository;
    public AddProductCommandHandler(IInventoryRepository inventoryRepository) => _inventoryRepository = inventoryRepository;
    public async Task<GeneralResponseDto<string>> Handle(AddProductCommand request, CancellationToken cancellationToken) => await _inventoryRepository.AddProductToDbAsync(request, cancellationToken);
}
