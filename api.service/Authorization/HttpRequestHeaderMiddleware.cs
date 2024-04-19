

using System.Net.Http.Headers;

namespace APIService.Authorization;

public class HttpRequestHeaderMiddleware // : IMiddleware
{
    // private readonly IHttpClientFactory _httpClientFactory;
    // private readonly IHttpContextAccessor _httpContextAccessor;

    // public HttpRequestHeaderMiddleware(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    // {
    //     _httpClientFactory = httpClientFactory;
    //     _httpContextAccessor = httpContextAccessor;
    // }

    // public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    // {
    //     var token = context.Request.Headers["Authorization"];

    //     if (!string.IsNullOrEmpty(token))
    //     {
    //         using (var httpClient = _httpClientFactory.CreateClient("defaultToken"))
    //         {
    //             var splitToken = token.ToString().Split(" ");
    //             httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", splitToken[1]);
    //             // Ensure _httpContextAccessor.HttpContext is not null before accessing its properties
    //             if (_httpContextAccessor.HttpContext != null)
    //             {
    //                 _httpContextAccessor.HttpContext.Items["defaultToken"] = httpClient;
    //             }
    //         }
    //     }

    //     await next(context);
    // }
}