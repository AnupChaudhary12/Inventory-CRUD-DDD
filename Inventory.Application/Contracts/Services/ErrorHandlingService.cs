using Inventory.Application.Contracts.DTOs;
using Inventory.Application.Contracts.Services.Interface;
using System.Net;

namespace Inventory.Application.Contracts.Services;

public class ErrorHandlingService : IErrorHandlingService
{
    public GeneralResponseDto<T> CreateFailureResponse<T>(string message, string errorCode, T? result = default)
    {
        int statusCode = MapErrorCodeToStatusCode(errorCode);

        return GeneralResponseDto<T>.FailureResponse(
            message: message,
            errorCode: errorCode,
            statusCode: statusCode
        );
    }

    private static int MapErrorCodeToStatusCode(string errorCode) => errorCode switch
    {
        "BAD_REQUEST" => (int)HttpStatusCode.BadRequest,
        "VALIDATION_ERROR" => (int)HttpStatusCode.BadRequest,
        "NOT_FOUND" => (int)HttpStatusCode.NotFound,
        "UNAUTHORIZED" => (int)HttpStatusCode.Unauthorized,
        "FORBIDDEN" => (int)HttpStatusCode.Forbidden,
        "DB_ERROR" => (int)HttpStatusCode.InternalServerError,
        "INTERNAL_ERROR" => (int)HttpStatusCode.InternalServerError,
        _ => (int)HttpStatusCode.InternalServerError 
    };
}
