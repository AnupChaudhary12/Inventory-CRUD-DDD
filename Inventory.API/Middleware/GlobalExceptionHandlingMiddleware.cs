using System.Net;
using System.Security;
using System.Text.Json;
using FluentValidation;
using Inventory.Application.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch(Exception ex) 
        {
            await HandleExceptionAsync(context, ex).ConfigureAwait(false);
        }
    }

    public async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(ex);
        (HttpStatusCode statusCode, string message, string errorCode, object? result) responseTuple = MapExceptionToResponse(ex);

        context.Response.StatusCode = (int)responseTuple.statusCode;
        context.Response.ContentType = "application/json";

        var response = GeneralResponseDto<object>.FailureResponse(
            message: responseTuple.message,
            errorCode: responseTuple.errorCode,
            statusCode: (int)responseTuple.statusCode
        );

        string? json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json).ConfigureAwait(false);

        // Log exception
        _logger.LogError(ex, "Exception caught in GlobalExceptionHandlingMiddleware: {Message}", ex.Message);
    }

    private (HttpStatusCode statusCode, string message, string errorCode, object? result) MapExceptionToResponse(Exception ex)
    {
        HttpStatusCode status;
        string message;
        string errorCode;
        object? result = null;

        switch (ex)
        {
            case ValidationException valEx:
                status = HttpStatusCode.BadRequest;
                message = "Validation failed.";
                errorCode = "VALIDATION_ERROR";
                result = valEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                break;

            case ArgumentNullException argNullEx:
                status = HttpStatusCode.BadRequest;
                message = argNullEx.Message;
                errorCode = "NULL_ARGUMENT";
                break;

            case ArgumentException argEx:
                status = HttpStatusCode.BadRequest;
                message = argEx.Message;
                errorCode = "INVALID_ARGUMENT";
                break;

            case FormatException formatEx:
                status = HttpStatusCode.BadRequest;
                message = formatEx.Message;
                errorCode = "FORMAT_ERROR";
                break;

            case OverflowException overflowEx:
                status = HttpStatusCode.BadRequest;
                message = overflowEx.Message;
                errorCode = "OVERFLOW_ERROR";
                break;

            case KeyNotFoundException keyEx:
                status = HttpStatusCode.NotFound;
                message = keyEx.Message;
                errorCode = "NOT_FOUND";
                break;

            case UnauthorizedAccessException:
                status = HttpStatusCode.Unauthorized;
                message = "Unauthorized access.";
                errorCode = "UNAUTHORIZED";
                break;

            case SecurityException:
                status = HttpStatusCode.Forbidden;
                message = "Forbidden access.";
                errorCode = "FORBIDDEN";
                break;

            case OperationCanceledException:
                status = HttpStatusCode.BadRequest;
                message = "Request was canceled.";
                errorCode = "REQUEST_CANCELED";
                break;

            case TimeoutException:
                status = HttpStatusCode.RequestTimeout;
                message = "Request was timeout";
                errorCode = "REQUEST_TIMEOUT";
                break;

            case InvalidOperationException invalidOpEx:
                status = HttpStatusCode.Conflict;
                message = invalidOpEx.Message;
                errorCode = "INVALID_OPERATION";
                break;

            case NotImplementedException:
                status = HttpStatusCode.NotImplemented;
                message = "Feature not implemented";
                errorCode = "NOT_IMPLEMENTED";
                break;

            case NotSupportedException:
                status = HttpStatusCode.NotImplemented;
                message = "Feature not supported.";
                errorCode = "NOT_SUPPORTED";
                break;

            case NullReferenceException:
                status = HttpStatusCode.InternalServerError;
                message = "Unexpected null reference.";
                errorCode = "NULL_REFERENCE";
                break;

            case IOException:
                status = HttpStatusCode.InternalServerError;
                message = "I/O error occurred.";
                errorCode = "IO_ERROR";
                break;

            case DbUpdateException:
                status = HttpStatusCode.InternalServerError;
                message = "Database update failed.";
                errorCode = "DB_ERROR";
                break;

            case JsonException:
                status = HttpStatusCode.BadRequest;
                message = "Malformed JSON.";
                errorCode = "JSON_ERROR";
                break;

            case HttpRequestException:
                status = HttpStatusCode.BadGateway;
                message = "Downstream service error.";
                errorCode = "HTTP_ERROR";
                break;

            default:
                status = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred.";
                errorCode = "INTERNAL_ERROR";
                break;
        }

        // Include stack trace in development only
        if (_env.IsDevelopment() && result == null)
        {
            result = ex.ToString();
        }

        return (status, message, errorCode, result);
    }
}
