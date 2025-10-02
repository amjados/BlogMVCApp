namespace BlogMVCApp.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public string? CorrelationId { get; set; }
    public int StatusCode { get; set; } = 500;
    public string? ErrorMessage { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public bool ShowCorrelationId => !string.IsNullOrEmpty(CorrelationId);

    public string StatusCodeText => StatusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        405 => "Method Not Allowed",
        500 => "Internal Server Error",
        502 => "Bad Gateway",
        503 => "Service Unavailable",
        _ => "Error"
    };
}
