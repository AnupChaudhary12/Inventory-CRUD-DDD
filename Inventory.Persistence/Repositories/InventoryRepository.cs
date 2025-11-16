using System.Net;
using Bogus;
using Inventory.Application.Contracts.DTOs;
using Inventory.Application.Contracts.PersistenceInterfaces;
using Inventory.Application.Features.Inventory.Command.AddProduct;
using Inventory.Application.Features.Inventory.Command.DeleteProduct;
using Inventory.Application.Features.Inventory.Command.UpdateProduct;
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

    public async Task<GeneralResponseDto<string>> DeleteProductAsync(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            _logger.LogError("Command is null while deleting product");
            return GeneralResponseDto<string>.FailureResponse(
                AppMessages.NullMessage("DeleteProductCommand"),
                "BAD_REQUEST",
                (int)HttpStatusCode.BadRequest
            );
        }

        Product? product = await _inventoryDbContext.Products
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

        if (product is null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", command.Id);
            return GeneralResponseDto<string>.FailureResponse(
                $"Product with ID {command.Id} not found.",
                "NOT_FOUND",
                (int)HttpStatusCode.NotFound
            );
        }

        _inventoryDbContext.Products.Remove(product);
        await _inventoryDbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product with ID {ProductId} deleted successfully", command.Id);

        return GeneralResponseDto<string>.SuccessResponse(
            $"Product with ID {command.Id} deleted successfully.",
            "SUCCESS",
            (int)HttpStatusCode.OK
        );
    }


    public async Task<GeneralResponseDto<List<GetProductDto>>> GetAllProducts(GetAllProductQuery? request, CancellationToken cancellationToken)
    {
        int pageNo = 1;
        int pageSize = 10;

        IQueryable<Product> query = _inventoryDbContext.Products.AsNoTracking();

        if (request is not null)
        {
            if (request.Id.HasValue)
            {
                query = query.Where(p => p.Id == request.Id.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.ProductName))
            {
                string productNameFilter = request.ProductName!.ToLower();
                query = query.Where(p => p.Name.Contains(productNameFilter, StringComparison.CurrentCultureIgnoreCase));
            }

            if (request.AvailableStockCountFrom.HasValue)
            {
                query = query.Where(p => p.AvailableStock >= request.AvailableStockCountFrom.Value);
            }

            if (request.AvailableStockCountTo.HasValue)
            {
                query = query.Where(p => p.AvailableStock <= request.AvailableStockCountTo.Value);
            }

            if (request.ReorderStockCountFrom.HasValue)
            {
                query = query.Where(p => p.ReorderStock >= request.ReorderStockCountFrom.Value);
            }

            if (request.ReorderStockCountTo.HasValue)
            {
                query = query.Where(p => p.ReorderStock <= request.ReorderStockCountTo.Value);
            }

            pageNo = request.PageNo ?? pageNo;
            pageSize = request.PageSize ?? pageSize;
        }


        int skip = (pageNo - 1) * pageSize;

        int totalCount = await query.CountAsync(cancellationToken);

        List<Product> products = await query
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (products.Count == 0)
        {
            _logger.LogInformation(AppMessages.NullMessage("List of Products"));
            return GeneralResponseDto<List<GetProductDto>>.FailureResponse(
                message: AppMessages.NullMessage("List of Products"),
                errorCode:"NOT_FOUND",
                statusCode: (int)HttpStatusCode.NotFound
            );
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

        _logger.LogInformation(AppMessages.RetrieveSuccessMessage("List of Products"));

        string message = $"{AppMessages.RetrieveSuccessMessage("List of Products")} " +
                         $"Page {pageNo} of {Math.Ceiling((double)totalCount / pageSize)} (Total: {totalCount})";

        return GeneralResponseDto<List<GetProductDto>>.SuccessResponse(
            result: getProducts,
            message: message,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<GeneralResponseDto<string>> UpdateProductAsync(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            _logger.LogError("Command is null while updating product");
            return GeneralResponseDto<string>.FailureResponse(
                AppMessages.NullMessage("UpdateProductCommand"),
                "BAD_REQUEST",
                (int)HttpStatusCode.BadRequest
            );
        }

        Product? existingProduct = await _inventoryDbContext.Products
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

        if (existingProduct is null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", command.Id);
            return GeneralResponseDto<string>.FailureResponse(
                $"Product with ID {command.Id} not found.",
                "NOT_FOUND",
                (int)HttpStatusCode.NotFound
            );
        }

        existingProduct.Update(
            command.Name,
            command.AvailabaleStock,
            command.ReorderStock
        );

        await _inventoryDbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product with ID {ProductId} updated successfully", command.Id);

        return GeneralResponseDto<string>.SuccessResponse(
            $"Product with ID {command.Id} updated successfully.",
            "SUCCESS",
            (int)HttpStatusCode.OK
        );
    }

    // Bogus and EfCore extension example to update in bulk 1 million products. Efcore Extensions is dual license and paid for commercial use when revenue is over 1 million uSd per annum. Free for test and individual.
    public async Task<GeneralResponseDto<string>> InsertBulkProductsUsingEfCoreExtensions(CancellationToken cancellationToken)
    {
        Faker<Product> faker = new Faker<Product>()
        .CustomInstantiator(f => Product.Create(
            f.Commerce.ProductName(),
            f.Random.Int(0, 1000),
            f.Random.Int(10, 100)
        ));
        List<Product> products = faker.Generate(1_000_000);

        await _inventoryDbContext.BulkInsertAsync(products,cancellationToken);

        _logger.LogInformation("1 million products added successfully");

        return GeneralResponseDto<string>.SuccessResponse(
            "1 million products added successfully.",
            "SUCCESS",
            (int)HttpStatusCode.OK
        );
    }
}
