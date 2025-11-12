using System.Net;

namespace Inventory.Shared;

public abstract class ApiException : Exception
{
    public string ErrorCode { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; }

    protected ApiException(string message, string errorCode, HttpStatusCode statusCode, string? details = null) : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
        Details = details;
        Timestamp = DateTime.UtcNow;
    }

}
