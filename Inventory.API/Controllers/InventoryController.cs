using Inventory.Application.Contracts.DTOs;
using Inventory.Application.Contracts.Services.Interface;
using Inventory.Application.Features.Inventory.Command.AddProduct;
using Inventory.Application.Features.Inventory.Query.GetAllProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IErrorHandlingService _errorHandlingService;
    public InventoryController(IMediator mediator, IErrorHandlingService errorHandlingService)
    {
        _mediator = mediator;
        _errorHandlingService = errorHandlingService;
    }

    [HttpGet("get-products")]
    public async Task<IActionResult> GetProducts([FromQuery]GetAllProductQuery query)
    {
        GeneralResponseDto<List<GetProductDto>> result = await _mediator.Send(query);

       if (result.Success)
        {
            return Ok(result);
        }

        GeneralResponseDto<List<GetProductDto>> errorResult = _errorHandlingService.CreateFailureResponse<List<GetProductDto>>(
            result.Message,
            result.ErrorCode ?? "INTERNAL_ERROR",
            result.Result
            );

        return StatusCode(errorResult.StatusCode, errorResult);
    }


    [HttpPost("add-product")]
    public async Task<IActionResult> AddProduct(AddProductCommand command)
    {
        GeneralResponseDto<string> result = await _mediator.Send(command);
        if (result.Success)
        {
            return Ok(result);
        }
        GeneralResponseDto<string> errorResult = _errorHandlingService.CreateFailureResponse<string>(
                result.Message,
                result.ErrorCode?? "INTERNAL_ERROR",
                result.Result
            );

        return StatusCode(errorResult.StatusCode, errorResult);
    }
}

