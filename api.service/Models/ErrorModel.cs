using System.Text.Json.Serialization;

namespace APIService.Models;
public class ErrorModel
{
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    public string? Details { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; }

    public ErrorModel(int statusCode, string? error, string? errorCode, string? message = null, string? details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
        Error = error;
        ErrorCode = errorCode;
    }
}