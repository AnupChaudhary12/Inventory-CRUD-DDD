using System.Net;
using Inventory.Application.Contracts.DTOs;
using Inventory.Application.Contracts.PersistenceInterfaces;
using Inventory.Application.Features.Inventory.Command.AddProduct;
using Inventory.Application.Features.Inventory.Query.GetAllProduct;
using Inventory.Domain.Entities;
using Inventory.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventory.Persistence.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _inventoryDbContext;
    private readonly ILogger<InventoryRepository> _logger;
    public InventoryRepository(InventoryDbContext inventoryDbContext, ILogger<InventoryRepository> logger)
    {
        _inventoryDbContext = inventoryDbContext;   
        _logger =logger;
    }
    public async Task<GeneralResponseDto<string>> AddProductToDbAsync(AddProductCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            _logger.LogError("Command is null while adding product");
            return GeneralResponseDto<string>.FailureResponse(
                AppMessages.NullMessage("AddProductCommand"),
                "BAD_REQUEST",
                (int)HttpStatusCode.BadRequest
            );
        }

        var product = Product.Create(command.Name, command.AvailableStock, command.ReorderStock);


        await _inventoryDbContext.Products.AddAsync(product, cancellationToken);
        await _inventoryDbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product added successfully");
        return GeneralResponseDto<string>.SuccessResponse(
            AppMessages.SuccessMessage("Product"),
            AppMessages.SuccessMessage(command.Name),
            (int)HttpStatusCode.OK
        );
    }

    public async Task<GeneralResponseDto<List<GetProductDto>>> GetAllProducts(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        List<Product> products = await _inventoryDbContext.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (products.Count == 0)
        {
            _logger.LogInformation(AppMessages.NullMessage("List of Products"));
        }

        var getProducts = products
            .Select(p => new GetProductDto
            {
                Id = p.Id,
                Name = p.Name,
                AvailableStock = p.AvailableStock,
                ReorderStock = p.ReorderStock
            })
            .ToList();

        _logger.LogInformation(AppMessages.RetrieveSuccessMessage("List of products"));
        return GeneralResponseDto<List<GetProductDto>>.SuccessResponse(
            result: getProducts,
            message: AppMessages.RetrieveSuccessMessage("List of Products"),
            statusCode: (int)HttpStatusCode.OK
        );
    }

}
