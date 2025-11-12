namespace Inventory.Application.Contracts.DTOs;

public class GeneralResponseDto<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public T? Result { get; set; }

    private GeneralResponseDto() { }

    public static GeneralResponseDto<T> SuccessResponse(
        T result,
        string message = "Request successful",
        int statusCode = 200)
        => new()
        {
            Success = true,
            StatusCode = statusCode,
            Message = message,
            Result = result
        };

    public static GeneralResponseDto<T> FailureResponse(
        string message,
        string? errorCode = null,
        int statusCode = 400)
        => new()
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            ErrorCode = errorCode
        };
}
