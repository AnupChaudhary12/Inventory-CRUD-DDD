using Inventory.Application.Contracts.DTOs;

namespace Inventory.Application.Contracts.Services.Interface;

public interface IErrorHandlingService
{
    GeneralResponseDto<T> CreateFailureResponse<T>(string message, string errorCode, T? result = default);
}
