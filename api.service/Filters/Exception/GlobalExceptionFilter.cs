
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using APIService.Models;

namespace APIService.Handlers;
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    private const string UnhandledExceptionMsg = "An unhandled exception has occurred while executing the request.";

    // private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    // {
    //     Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    // };

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
        $"An error occurred while processing your request: {exception.Message}");
        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Type = exception.GetType().Name,
            Title = "An unhandled error occurred",
            Detail = exception.Message
        };

        var requestData = new RequestData { RequestId = Guid.NewGuid() }; // Generate a unique request ID
        var responseData = new ResponseData<object> // Assuming object is your generic type
        {
            Status = "error",
            StatusDetails = null,
            DataObjs = problemDetails
        };

        var wrappedResponse = new ApiResponse<object>(requestData, responseData);

        await httpContext
            .Response
            .WriteAsJsonAsync(wrappedResponse, cancellationToken);
        return true;
    }

    // private ProblemDetails CreateProblemDetails(in HttpContext context, in Exception exception)
    // {
    //     var errorCode = exception.;
    //     var statusCode = context.Response.StatusCode;
    //     var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);
    //     if (string.IsNullOrEmpty(reasonPhrase))
    //     {
    //         reasonPhrase = UnhandledExceptionMsg;
    //     }

    //     var problemDetails = new ProblemDetails
    //     {
    //         Status = statusCode,
    //         Title = reasonPhrase,
    //         Extensions =
    //         {
    //             [nameof(errorCode)] = errorCode
    //         }
    //     };

    //     // if (!env.IsDevelopmentOrQA())
    //     // {
    //     //     return problemDetails;
    //     // }

    //     problemDetails.Detail = exception.ToString();
    //     problemDetails.Extensions["traceId"] = context.TraceIdentifier;
    //     problemDetails.Extensions["data"] = exception.Data;

    //     return problemDetails;
    // }

    // private string ToJson(in ProblemDetails problemDetails)
    // {
    //     try
    //     {
    //         return JsonSerializer.Serialize(problemDetails, SerializerOptions);
    //     }
    //     catch (Exception ex)
    //     {
    //         const string msg = "An exception has occurred while serializing error to JSON";
    //         logger.LogError(ex, msg);
    //     }

    //     return string.Empty;
    // }
}