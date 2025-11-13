using System.Threading;
using Inventory.Application.Contracts.DTOs;
using Inventory.Application.Features.Inventory.Command.AddProduct;
using Inventory.Application.Features.Inventory.Query.GetAllProduct;
using MediatR;

namespace Inventory.Application.Contracts.PersistenceInterfaces;

public interface IInventoryRepository
{
    Task<GeneralResponseDto<string>> AddProductToDbAsync(AddProductCommand command, CancellationToken cancellationToken);
    Task<GeneralResponseDto<List<GetProductDto>>> GetAllProducts(GetAllProductQuery? request, CancellationToken cancellationToken);
}
