namespace APIService.Models;

public class ApiResponse<T>
{
    public RequestData Request { get; set; }
    public ResponseData<T> Response { get; set; }

    public ApiResponse(RequestData request, ResponseData<T> response)
    {
        Request = request;
        Response = response;
    }
}