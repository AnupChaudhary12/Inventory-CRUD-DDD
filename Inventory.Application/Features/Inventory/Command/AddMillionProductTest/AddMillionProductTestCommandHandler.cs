using Inventory.Application.Contracts.DTOs;
using Inventory.Application.Contracts.PersistenceInterfaces;
using MediatR;

namespace Inventory.Application.Features.Inventory.Command.AddMillionProductTest;

public class AddMillionProductTestCommandHandler : IRequestHandler<AddMillionProductTestCommand, GeneralResponseDto<string>>
{
    private readonly IInventoryRepository _inventoryRepository;
    public AddMillionProductTestCommandHandler(IInventoryRepository inventoryRepository) => _inventoryRepository = inventoryRepository;

    public async Task<GeneralResponseDto<string>> Handle(AddMillionProductTestCommand request, CancellationToken cancellationToken) => await _inventoryRepository.InsertBulkProductsUsingEfCoreExtensions(cancellationToken);
}
